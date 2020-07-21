using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            IngredientInfo = new HashSet<IngredientInfo>();
        }

        public int IngredientId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }

        public ICollection<IngredientInfo> IngredientInfo { get; set; }
    }
}
