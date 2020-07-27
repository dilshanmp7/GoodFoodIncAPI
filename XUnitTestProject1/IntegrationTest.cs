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

        protected IntegrationTest()
        {
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
                        
                    });
                });
            TestClient = applicationFactory.CreateClient();
        }

    }
}
