using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GoodFoodIncAPI;
using GoodFoodIncAPI.Models;
using GoodFoodIncAPI.ResponseModel;
using IntegrationTest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1
{
    public class IngredientsControllerTest : IntegrationTest
    {

        public IngredientsControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {

        }
        
        [Theory]
        [InlineData("/Ingredients")]
        public async void GetAllIngredients_WithoutAnyIngredients_ReturnEmpty(string url)
        {
            //Act
            var response = await Client.GetAsync(url);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Ingredient>>()).Should().BeEmpty();
        }

        [Theory]
        [InlineData("/Ingredients")]
        public async void CeateIngredients_ReturnIngredients(string url)
        {
            //Act
            IngredientRespondModel testIngredient = new IngredientRespondModel()
            {
                Title = "Test_Ingredient_1",
                Description = "This is a Test value",
                Owner = "testUser"
            };
            var stringIngredient = await Task.Run(() => JsonConvert.SerializeObject(testIngredient));
            var httpContent = new StringContent(stringIngredient, Encoding.UTF8, "application/json");
            var postResponse = await Client.PostAsync(url, httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await Client.GetAsync(url);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getResponse.Content.ReadAsAsync<List<IngredientRespondModel>>()).Should().HaveCount(1);
        }

        [Theory]
        [InlineData("/Ingredients")]
        public async void UpdateIngredients_WhenExistsInDB(string url)
        {
            //Act
            IngredientRespondModel testIngredient = new IngredientRespondModel()
            {
                Title = "Test_Ingredient_1",
                Description = "This is a Test value",
                Owner = "testUser" 
            };
            var stringIngredient = await Task.Run(() => JsonConvert.SerializeObject(testIngredient));
            var httpContent = new StringContent(stringIngredient, Encoding.UTF8, "application/json");
            var postResponse = await Client.PostAsync(url, httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var existingItems = JsonConvert.DeserializeObject<List<IngredientRespondModel>>(responseBody);

            existingItems.First().Title = "Test_Ingredient_2";
            existingItems.First().Description = "This is a Test value2";

            stringIngredient = await Task.Run(() => JsonConvert.SerializeObject(existingItems.First()));
            httpContent = new StringContent(stringIngredient, Encoding.UTF8, "application/json");
            var putResponse = await Client.PutAsync($"{url}/1", httpContent);

            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            response = await Client.GetAsync($"{url}/1");
            response.EnsureSuccessStatusCode();
            responseBody = await response.Content.ReadAsStringAsync();

            var existingItem = JsonConvert.DeserializeObject<IngredientRespondModel>(responseBody);

            Assert.Equal("Test_Ingredient_2", existingItem.Title);
            Assert.Equal("This is a Test value2", existingItem.Description);
        }

        [Theory]
        [InlineData("/Ingredients")]
        public async void DeleteIngredients_WhenExistsInDB(string url)
        {
            //Act
            IngredientRespondModel testIngredient = new IngredientRespondModel()
            {
                Title = "Test_Ingredient_1",
                Description = "This is a Test value",
                Owner = "testUser"
            };
            var stringIngredient = await Task.Run(() => JsonConvert.SerializeObject(testIngredient));
            var httpContent = new StringContent(stringIngredient, Encoding.UTF8, "application/json");
            var postResponse = await Client.PostAsync(url, httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await Client.GetAsync(url);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getResponse.Content.ReadAsAsync<List<IngredientRespondModel>>()).Should().HaveCount(1);

            var deleteResponse = await Client.DeleteAsync($"{url}/1");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            getResponse = await Client.GetAsync(url);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getResponse.Content.ReadAsAsync<List<IngredientRespondModel>>()).Should().HaveCount(0);


        }


        
    }
}
