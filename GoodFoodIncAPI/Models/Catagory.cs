using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public partial class Catagory
    {
        public Catagory()
        {
            Recipes = new HashSet<Recipe>();
        }

        public int CatagoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
