using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleMvcApp.Support;
using SampleMvcApp.Controllers; // Ensure this is added
using StackExchange.Redis;
using System.Net.Http;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.CallbackPath = "/callback";
    options.LoginParameters.Add("organization", builder.Configuration["Auth0:Organization"]);
});

// Register the IHttpClientFactory
builder.Services.AddHttpClient();

// Register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
    ConnectionMultiplexer.Connect("localhost"));

// Register auth0Domain and accessToken
var auth0Domain = builder.Configuration["Auth0:Domain"];
var accessToken = builder.Configuration["Auth0:ManagementAccessToken"];
builder.Services.AddSingleton(auth0Domain);
builder.Services.AddSingleton(accessToken);

// Register Auth0ManagementApiService with a factory method
builder.Services.AddTransient<Auth0ManagementApiService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    var domain = sp.GetRequiredService<string>(); // This should retrieve the domain
    var token = sp.GetRequiredService<string>();  // This should retrieve the token

    if (string.IsNullOrEmpty(domain))
    {
        throw new ArgumentException("Auth0 domain is not set or is invalid.");
    }

    return new Auth0ManagementApiService(httpClient, domain, token);
});



// Configure the HTTP request pipeline.
builder.Services.ConfigureSameSiteNoneCookies();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
