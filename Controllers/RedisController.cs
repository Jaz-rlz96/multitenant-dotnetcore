using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleMvcApp.ViewModels;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace SampleMvcApp.Controllers
{
    public class RedisController : Controller
    {
        private readonly IDatabase _redisDb;

        public RedisController(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public IActionResult Index()
        {
            // For demonstration, let's fetch two users with IDs 1 and 2
            var user1Fields = _redisDb.HashGetAll("user:1");
            var user2Fields = _redisDb.HashGetAll("user:2");

            var users = new List<User>
        {
            new User
            {
                ID = "1",
                Name = user1Fields.Single(f => f.Name == "Name").Value,
                Age = int.Parse(user1Fields.Single(f => f.Name == "Age").Value)
            },
            new User
            {
                ID = "2",
                Name = user2Fields.Single(f => f.Name == "Name").Value,
                Age = int.Parse(user2Fields.Single(f => f.Name == "Age").Value)
            }
        };

            var viewModel = new UserViewModelRedis { Users = users };

            return View("~/Views/Account/RedisUsers.cshtml", viewModel);
        }

}
}
