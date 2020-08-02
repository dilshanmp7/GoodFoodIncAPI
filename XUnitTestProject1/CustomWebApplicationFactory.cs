using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GoodFoodIncAPI;
using GoodFoodIncAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected GoodFoodIncDBContext Context;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault( d => d.ServiceType ==
                         typeof(DbContextOptions<GoodFoodIncDBContext>));

                services.Remove(descriptor);

                services.AddDbContext<GoodFoodIncDBContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    Context = scopedServices.GetRequiredService<GoodFoodIncDBContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    Context.Database.EnsureCreated();
                }
            });
        }

    }

    public class Utilities
    {
        public enum CatagoryTypes
        {
            starters,
            [Display(Name = "main course")]
            maincourse,
            dessert
        }
        
        public static void InitializeUserAndCatagoryTestData(GoodFoodIncDBContext db)
        {
            db.Users.Add(new User() { UserName = "testUser", Password = "12345" });
            db.Catagories.Add(new Catagory() { Name = CatagoryTypes.starters.ToString() });
            db.Catagories.Add(new Catagory() { Name = CatagoryTypes.maincourse.ToString() });
            db.Catagories.Add(new Catagory() { Name = CatagoryTypes.dessert.ToString() });
            db.SaveChanges();
        }

        public static void InitializeIngredientTestData(GoodFoodIncDBContext db)
        {
            db.Ingredients.Add(new Ingredient(){Title = "milk",Description = "Sød Milk"});
            db.Ingredients.Add(new Ingredient() { Title = "egg", Description = "white egg" });
            db.SaveChanges();
        }
    }
}
