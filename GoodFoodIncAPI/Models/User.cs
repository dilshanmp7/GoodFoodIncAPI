using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public partial class User
    {
        public User()
        {
            Ingredients = new HashSet<Ingredient>();
            Recipes = new HashSet<Recipe>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
