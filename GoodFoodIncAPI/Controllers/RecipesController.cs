using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodFoodIncAPI.Models;
using GoodFoodIncAPI.Response;

namespace GoodFoodIncAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly GoodFoodIncDBContext _context;

        public RecipesController(GoodFoodIncDBContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public IEnumerable<RecipesRespondModel> GetRecipes()
        {
            var recipesModels = new List<RecipesRespondModel>();
            foreach (var recipe in _context.Recipes.ToList())
            {
                var recipeModel = InitializeRecipesRespondModel(recipe);
                recipesModels.Add(recipeModel);
            }
            return recipesModels;
        }

        private RecipesRespondModel InitializeRecipesRespondModel(Recipe recipe)
        {
            var recipeModel = new RecipesRespondModel
            {
                Title = recipe.Title,
                Description = recipe.Description,
                Category = _context.Catagories.First(a => a.CatagoryId.Equals(recipe.CatagoryId)).Name,
                Owner = _context.Users.First(a => a.UserId.Equals(recipe.UserId)).UserName
            };
            var queryable = _context.IngredientInfoes.Where(a => a.RecipeId.Equals(recipe.RecipeId))
                .Select(a => new {a.IngredientId,a.Qty});
            foreach (var kvp in queryable)
            {
                recipeModel.Ingredients.Add(new KeyValuePair<string, string>(_context.Ingredients.First(a => a.IngredientId.Equals(kvp.IngredientId)).Title,kvp.Qty));
            }
            return recipeModel;
        }
        
        // GET: api/Recipes/5
        [HttpGet("{userId}/users")]
        public async Task<IActionResult> GetRecipesByUserId([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipes =  _context.Recipes.Where(a=>a.UserId.Equals(userId));

            if (recipes == null || !recipes.Any())
            {
                return NotFound();
            }

            var recipesModels = new List<RecipesRespondModel>();
            foreach (var recipe in recipes)
            {
                var recipeModel = InitializeRecipesRespondModel(recipe);
                recipesModels.Add(recipeModel);
            }
            return Ok(recipesModels);
        }




        // GET: api/Recipes/5
        [HttpGet("{id}")] 
        public async Task<IActionResult> GetRecipeByID([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }
            var recipeModel = InitializeRecipesRespondModel(recipe);

            return Ok(recipeModel);
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe([FromRoute] int id, [FromBody]RecipesRespondModel recipeRespondModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(recipeRespondModel.Title))
            {
                return BadRequest("Invalid Title");
            }
            
            var recipe = await _context.Recipes.FindAsync(id);

            recipe.Title = recipeRespondModel.Title;
            recipe.Catagory = _context.Catagories.First(a=>recipeRespondModel != null && a.Name.ToLower().Equals(recipeRespondModel.Category.ToLower())) ;
            recipe.Description = recipeRespondModel.Description;

            
            // delete IngredientInfoes 
            DeleteRelevantIngredentInfosDetails(id);

            var recipeIngredentsInfo = new List<IngredientInfo>();

            foreach (var kvp in recipeRespondModel.Ingredients)
            {
                var ingredient = _context.Ingredients.FirstOrDefault(a => a.Title.ToLower().Equals(kvp.Key.ToLower()));
                var ingredientInfo = new IngredientInfo() { Ingredient = ingredient, Qty = kvp.Value, Recipe = recipe };
                recipeIngredentsInfo.Add(ingredientInfo);
            }

            recipe.IngredientInfoes = recipeIngredentsInfo;
            await _context.IngredientInfoes.AddRangeAsync(recipeIngredentsInfo);
            ///
            /// 
            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private void DeleteRelevantIngredentInfosDetails(int id)
        {
            var selectedIngredientInfoes = _context.IngredientInfoes.Where(a => a.RecipeId.Equals(id));
            _context.IngredientInfoes.RemoveRange(selectedIngredientInfoes);
        }

        // POST: api/Recipes
        [HttpPost]
        public async Task<IActionResult> SaveRecipe([FromBody] RecipesRespondModel recipeRespondModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(a => a.UserName.Equals(recipeRespondModel.Owner));
            if (user == null)
            {
                return BadRequest($"Invalid User {recipeRespondModel.Owner}");
            }

            var category = _context.Catagories.FirstOrDefault(a => a.Name.Trim().ToLower().Equals(recipeRespondModel.Category.Trim().ToLower()));
            if (category == null)
            {
                return BadRequest($"Invalid Catagory {recipeRespondModel.Category}");
            }
            
            var recipe = new Recipe()
            {
                Title = recipeRespondModel.Title,
                Description = recipeRespondModel.Description,
                Catagory = category,
                User = user,
            };
            
            foreach (var kvp in recipeRespondModel.Ingredients)
            {
                var ingredient =  _context.Ingredients.FirstOrDefault(a=>a.Title.ToLower().Equals(kvp.Key.ToLower()));
                var ingredientInfo = new IngredientInfo() { Ingredient = ingredient, Qty = kvp.Value, Recipe = recipe};
                recipe.IngredientInfoes.Add(ingredientInfo);
                await _context.IngredientInfoes.AddAsync(ingredientInfo);
            }

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipeByID", new { id = recipe.RecipeId }, recipeRespondModel);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            //remove dependencies 
            DeleteRelevantIngredentInfosDetails(id);


            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok(recipe);
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }
    }
}