using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GoodFoodIncAPI.Models;
using GoodFoodIncAPI.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1
{
    public class IngredientsControllerTest : IntegrationTest
    {
        [Fact]
        public async void GetAllIngredients_WithoutAnyIngredients_ReturnEmpty()
        {
            //Act
            var response = await TestClient.GetAsync("https://localhost:44300/Ingredients");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Ingredient>>()).Should().BeEmpty();
        }

        [Fact]
        public async void CeateIngredients_ReturnIngredients_WhenExistsInDB()
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
            var PostResponse = await TestClient.PostAsync("https://localhost:44300/Ingredients", httpContent);

            //Assert
            PostResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getresponse = await TestClient.GetAsync("https://localhost:44300/Ingredients");

            getresponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getresponse.Content.ReadAsAsync<List<IngredientRespondModel>>()).Should().HaveCount(1);
        }


        [Fact]
        public async void UpdateIngredients_ReturnIngredients_WhenExistsInDB()
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
            var postResponse = await TestClient.PostAsync("https://localhost:44300/Ingredients", httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await TestClient.GetAsync("https://localhost:44300/Ingredients");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getResponse.Content.ReadAsAsync<List<IngredientRespondModel>>()).Should().HaveCount(1);
        }

        [Fact]
        public async void UpdateIngredients_WhenExistsInDB()
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
            var postResponse = await TestClient.PostAsync("https://localhost:44300/Ingredients", httpContent);

            //Assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await TestClient.GetAsync("https://localhost:44300/Ingredients");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var existingItems = JsonConvert.DeserializeObject<List<IngredientRespondModel>>(responseBody);

            existingItems.First().Title = "Test_Ingredient_2";
            existingItems.First().Description = "This is a Test value2";

            stringIngredient = await Task.Run(() => JsonConvert.SerializeObject(existingItems.First()));
            httpContent = new StringContent(stringIngredient, Encoding.UTF8, "application/json");
            var putResponse = await TestClient.PutAsync($"https://localhost:44300/Ingredients/1", httpContent);

            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            response = await TestClient.GetAsync("https://localhost:44300/Ingredients");
            response.EnsureSuccessStatusCode();
            responseBody = await response.Content.ReadAsStringAsync();

            existingItems = JsonConvert.DeserializeObject<List<IngredientRespondModel>>(responseBody);

            Assert.Equal("Test_Ingredient_2", existingItems.First().Title);
            Assert.Equal("This is a Test value2", existingItems.First().Description);
        }

    }
}
