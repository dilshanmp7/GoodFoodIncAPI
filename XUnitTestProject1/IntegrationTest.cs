using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GoodFoodIncAPI;
using GoodFoodIncAPI.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace XUnitTestProject1
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        protected GoodFoodIncDBContext Context;

        protected IntegrationTest()
        {
            ServiceProvider sp = null;
            var applicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(GoodFoodIncDBContext));
                        services.AddDbContext<GoodFoodIncDBContext>(opt =>
                        {
                            opt.UseInMemoryDatabase("TestDB");
                        });
                        sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            var scopeServiceProvider = scope.ServiceProvider;
                            Context = scopeServiceProvider.GetRequiredService<GoodFoodIncDBContext>();
                            Context.Database.EnsureCreated();
                            InitializeDB(Context);
                        }
                    });
                });

            TestClient = applicationFactory.CreateClient();
        }

        private void InitializeDB(GoodFoodIncDBContext db)
        {
            // add defult test user
            db.Users.Add(new User() {UserName = "testUser", Password = "12345"});
            db.Catagories.Add(new Catagory(){Name = "starters" });
            db.Catagories.Add(new Catagory() { Name = "main course" });
            db.Catagories.Add(new Catagory() { Name = "dessert" });
            db.SaveChanges();
        }
    }
}
