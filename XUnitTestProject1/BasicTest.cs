using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodFoodIncAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTest
{
    public class BasicTest :IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/Ingredients")]
        [InlineData("/Recipes")]
        public async Task GetHttpRequest(string url)
        {
           // Arrange
           var client = _factory.CreateClient();

           //Act
           var response = await client.GetAsync(url);

           // Assert
           response.EnsureSuccessStatusCode();
           Assert.Equal("application/json; charset=utf-8",response.Content.Headers.ContentType.ToString());

        }

    }
}
