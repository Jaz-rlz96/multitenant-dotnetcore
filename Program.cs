using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleMvcApp.Support;
using SampleMvcApp.Controllers;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System;

namespace SampleMvcApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"];
                options.ClientId = builder.Configuration["Auth0:ClientId"];
                options.CallbackPath = "/callback";
                options.LoginParameters.Add("organization", builder.Configuration["Auth0:Organization"]);
            });

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("localhost"));

            builder.Services.Configure<Auth0Settings>(builder.Configuration.GetSection("Auth0"));

            builder.Services.AddTransient<Auth0ManagementApiService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();

                var auth0Settings = sp.GetRequiredService<IOptions<Auth0Settings>>().Value;
                var domain = auth0Settings.Domain;
                var token = auth0Settings.ManagementAccessToken;

                if (string.IsNullOrEmpty(domain))
                {
                    throw new ArgumentException("Auth0 domain is not set or is invalid.");
                }

                return new Auth0ManagementApiService(httpClient, domain, token);
            });

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
        }

        public class Auth0Settings
        {
            public string Domain { get; set; }
            public string ManagementAccessToken { get; set; }
        }
    }
}
