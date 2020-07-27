using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GoodFoodIncAPI.Models;
using GoodFoodIncAPI.Response;
using GoodFoodIncAPI.ResponseModel;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1
{
    public class RecipesControllerTest : IntegrationTest
    {

        private enum CatagoryTypes
        {
            starters,
            [Display(Name = "main course")]
            maincourse,
            dessert
        }
        
        [Fact]
        public async void GetAllRecipes_WithoutAnyRecipes_ReturnEmpty()
        {
            //Act
            var response = await TestClient.GetAsync("https://localhost:44300/Recipes");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<RecipesRespondModel>>()).Should().BeEmpty();
        }

        [Fact]
        public async void CeateRecipes_ReturnRecipes_WhenExistsInDB()
        {
            //Act
            RecipesRespondModel testRespondModel = new RecipesRespondModel()
            {
                Ingredients = new Dictionary<string, string>()
                {
                    {"milk","1l"},
                    {"egg","3"}
                },
                Category = CatagoryTypes.dessert.ToString(),
                Title = "Test_Recipe_1",
                Description = "This is a Test value",
                Owner = "testUser"
            };

            var stringRecipes = await Task.Run(() => JsonConvert.SerializeObject(testRespondModel));
            var httpContent = new StringContent(stringRecipes, Encoding.UTF8, "application/json");
            var PostResponse = await TestClient.PostAsync("https://localhost:44300/Recipes", httpContent);

            //Assert
            PostResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getresponse = await TestClient.GetAsync("https://localhost:44300/Recipes");

            getresponse.StatusCode.Should().Be(HttpStatusCode.OK);
            (await getresponse.Content.ReadAsAsync<List<RecipesRespondModel>>()).Should().HaveCount(1);
        }

    }
}
