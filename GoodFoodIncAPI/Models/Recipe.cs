using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            IngredientInfo = new HashSet<IngredientInfo>();
        }

        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int CatagoryId { get; set; }
        public string Description { get; set; }
        public int IngredientInfoId { get; set; }

        public Catagory Catagory { get; set; }
        public IngredientInfo IngredientInfoNavigation { get; set; }
        public ICollection<IngredientInfo> IngredientInfo { get; set; }
    }
}
