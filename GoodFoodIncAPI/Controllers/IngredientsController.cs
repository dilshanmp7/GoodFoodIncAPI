using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodFoodIncAPI.Models;
using GoodFoodIncAPI.Response;
using GoodFoodIncAPI.ResponseModel;

namespace GoodFoodIncAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly GoodFoodIncDBContext _context;

        public IngredientsController(GoodFoodIncDBContext context)
        {
            _context = context;
        }

        // GET: api/Ingredients
        [HttpGet]
        public IEnumerable<IngredientRespondModel> GetAllIngredients()
        {
            var ingredientRespondModels = new List<IngredientRespondModel>();
            foreach (var ingredient in _context.Ingredients.ToList())
            {
                var recipeModel = InitializeIngredientRespondModel(ingredient);
                ingredientRespondModels.Add(recipeModel);
            }
            return ingredientRespondModels;
        }

        private IngredientRespondModel InitializeIngredientRespondModel(Ingredient ingredient)
        {
            var ingredientRespondModel = new IngredientRespondModel
            {
                Title = ingredient.Title,
                Description = ingredient.Description,
                Owner = _context.Users.FirstOrDefault(a => a.UserId.Equals(ingredient.UserId))?.UserName
            };
            return ingredientRespondModel;
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngredientByID([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }
            var ingredientRespondModel = InitializeIngredientRespondModel(ingredient);
            return Ok(ingredientRespondModel);
        }


        [HttpGet("{userId}/users")]
        public async Task<IActionResult> GetIngredientsByUserId([FromRoute] int userId)
        {
            //if (!ModelState.IsValid)
            //{
            //    return new List<Ingredient>();
            //}
            //return await _context.Ingredients.Where(a => a.UserId.Equals(userId)).ToListAsync();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingredients = _context.Ingredients.Where(a => a.UserId.Equals(userId));

            if (ingredients == null || !ingredients.Any())
            {
                return NotFound();
            }

            var ingredientRespondModels = new List<IngredientRespondModel>();
            foreach (var ingredient in ingredients)
            {
                var ingredientRespondModel = InitializeIngredientRespondModel(ingredient);
                ingredientRespondModels.Add(ingredientRespondModel);
            }
            return Ok(ingredientRespondModels);

        }

        // PUT: Ingredients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient([FromRoute] int id, [FromBody] IngredientRespondModel ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingIngredient = await _context.Ingredients.FindAsync(id);

            existingIngredient.Title = ingredient.Title;
            existingIngredient.Description = ingredient.Description;

            _context.Entry(existingIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        // POST: Ingredients
        [HttpPost]
        public async Task<IActionResult> CreateIngredient([FromBody] IngredientRespondModel ingredientRespondModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingredient = new Ingredient()
            {
                Title = ingredientRespondModel.Title,
                Description = ingredientRespondModel.Description
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredientByID", new { id = ingredient.IngredientId }, ingredient);
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return Ok(ingredient);
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.IngredientId == id);
        }
    }
}