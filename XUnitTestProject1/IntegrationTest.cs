using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GoodFoodIncAPI;
using GoodFoodIncAPI.Models;
using IntegrationTest;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace XUnitTestProject1
{
    public class IntegrationTest: IClassFixture<CustomWebApplicationFactory<Startup>>
    {

        protected readonly HttpClient Client;
        protected GoodFoodIncDBContext Context;
        protected readonly CustomWebApplicationFactory<Startup> Factory;

        public IntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            Factory = factory;
            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });
            var scopeFactory = Factory.Server.Host.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            Context = scope.ServiceProvider.GetService<GoodFoodIncDBContext>();
        }


        [Theory]
        [InlineData("/Ingredients")]
        [InlineData("/Recipes")]
        public async Task GetHttpRequest(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

        }

    }
}
