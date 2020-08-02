using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GoodFoodIncAPI;
using GoodFoodIncAPI.Controllers;
using GoodFoodIncAPI.Models;
using GoodFoodIncAPI.Response;
using GoodFoodIncAPI.ResponseModel;
using IntegrationTest;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1
{
    public class RecipesControllerTest : IntegrationTest
    {
        public RecipesControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("/Recipes")]
        public async void GetAllRecipes_WithoutAnyRecipes_ReturnEmpty(string url)
        {
            //Act
            var response = await Client.GetAsync(url);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<RecipesRespondModel>>()).Should().BeEmpty();
        }

        [Theory]
        [InlineData("/Recipes")]
        public async void PostRecipes_ReturnRecipes_WhenExistsInDB(string url)
        {
            //Act
            Utilities.InitializeUserAndCatagoryTestData(Context);
            Utilities.InitializeIngredientTestData(Context);
            RecipesRespondModel testRespondModel = new RecipesRespondModel()
            {
                Ingredients = new Dictionary<string, string>()
                {
                    {"milk","1l"},
                    {"egg","3"}
                },
                Category = Utilities.CatagoryTypes.dessert.ToString(),
                Title = "Test_Recipe_1",
                Description = "This is a Test value",
                Owner = "testUser"
            };

            var stringRecipes = await Task.Run(() => JsonConvert.SerializeObject(testRespondModel));
            var httpContent = new StringContent(stringRecipes, Encoding.UTF8, "application/json");
            var postResponse = await Client.PostAsync(url, httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await Client.GetAsync(url);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getResponse.Content.ReadAsAsync<List<RecipesRespondModel>>()).Should().HaveCount(1);
        }


        [Theory]
        [InlineData("/Recipes")]
        public async void UpdateRecipes_WhenExistsInDB(string url)
        {
            //Act
            Utilities.InitializeUserAndCatagoryTestData(Context);
            Utilities.InitializeIngredientTestData(Context);
            RecipesRespondModel testRespondModel = new RecipesRespondModel()
            {
                Ingredients = new Dictionary<string, string>()
                {
                    {"milk","1l"},
                    {"egg","3"}
                },
                Category = Utilities.CatagoryTypes.dessert.ToString(),
                Title = "Test_Recipe_1",
                Description = "This is a Test value",
                Owner = "testUser"
            };

            var stringRecipes = await Task.Run(() => JsonConvert.SerializeObject(testRespondModel));
            var httpContent = new StringContent(stringRecipes, Encoding.UTF8, "application/json");
            var postResponse = await Client.PostAsync(url, httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await Client.GetAsync($"{url}/1");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var existingItem = JsonConvert.DeserializeObject<RecipesRespondModel>(await getResponse.Content.ReadAsStringAsync());

            existingItem.Title = "Test_Recipe_Edit";
            existingItem.Description = "This is edit value";
            existingItem.Category = Utilities.CatagoryTypes.starters.ToString();
            existingItem.Ingredients = new Dictionary<string, string>()
            {
                {"milk", "5l"},
                {"egg", "10"}
            };

            stringRecipes = await Task.Run(() => JsonConvert.SerializeObject(existingItem));
            httpContent = new StringContent(stringRecipes, Encoding.UTF8, "application/json");

            var putResponse = await Client.PutAsync($"{url}/1", httpContent);
            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResponse = await Client.GetAsync($"{url}/1");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            existingItem = JsonConvert.DeserializeObject<RecipesRespondModel>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal("Test_Recipe_Edit", existingItem.Title);
            Assert.Equal("This is edit value", existingItem.Description);
            Assert.Equal(Utilities.CatagoryTypes.starters.ToString(), existingItem.Category);
            Assert.True(existingItem.Ingredients.Keys.Contains("milk"));
            Assert.True(existingItem.Ingredients.Keys.Contains("egg"));
            Assert.Equal("5l", existingItem.Ingredients["milk"]);
            Assert.Equal("10", existingItem.Ingredients["egg"]);

        }

        [Theory]
        [InlineData("/Recipes")]
        public async void DeleteRecipes_WhenExistsInDB(string url)
        {
            //Act
            Utilities.InitializeUserAndCatagoryTestData(Context);
            Utilities.InitializeIngredientTestData(Context);
            RecipesRespondModel testRespondModel = new RecipesRespondModel()
            {
                Ingredients = new Dictionary<string, string>()
                {
                    {"milk","1l"},
                    {"egg","3"}
                },
                Category = Utilities.CatagoryTypes.dessert.ToString(),
                Title = "Test_Recipe_1",
                Description = "This is a Test value",
                Owner = "testUser"
            };

            var stringRecipes = await Task.Run(() => JsonConvert.SerializeObject(testRespondModel));
            var httpContent = new StringContent(stringRecipes, Encoding.UTF8, "application/json");
            var postResponse = await Client.PostAsync(url, httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await Client.GetAsync(url);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getResponse.Content.ReadAsAsync<List<RecipesRespondModel>>()).Should().HaveCount(1);

            var deleteResponse = await Client.DeleteAsync($"{url}/1");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            getResponse = await Client.GetAsync(url);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getResponse.Content.ReadAsAsync<List<RecipesRespondModel>>()).Should().HaveCount(0);

        }

        [Theory]
        [InlineData("/Recipes")]
        public async void GetRecipesByUserID_WhenExistsInDB(string url)
        {
            //Act
            Utilities.InitializeUserAndCatagoryTestData(Context);
            Utilities.InitializeIngredientTestData(Context);
            RecipesRespondModel testRespondModel = new RecipesRespondModel()
            {
                Ingredients = new Dictionary<string, string>()
                {
                    {"milk","1l"},
                    {"egg","3"}
                },
                Category = Utilities.CatagoryTypes.dessert.ToString(),
                Title = "Test_Recipe_1",
                Description = "This is a Test value",
                Owner = "testUser"
            };

            var stringRecipes = await Task.Run(() => JsonConvert.SerializeObject(testRespondModel));
            var httpContent = new StringContent(stringRecipes, Encoding.UTF8, "application/json");
            var postResponse = await Client.PostAsync(url, httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await Client.GetAsync($"{url}/1/users");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var existingItems = JsonConvert.DeserializeObject<List<RecipesRespondModel>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal("testUser",existingItems.First().Owner);

        }

        
    }
}
