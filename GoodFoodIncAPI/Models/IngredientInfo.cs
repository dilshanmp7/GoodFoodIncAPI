using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public partial class IngredientInfo
    {
        public int IngredientInfoId { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Qty { get; set; }

        public Ingredient Ingredient { get; set; }
        public Recipe Recipe { get; set; }
    }
}
