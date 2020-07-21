using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public partial class Catagory
    {
        public Catagory()
        {
            Recipe = new HashSet<Recipe>();
        }

        public int CatagoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Recipe> Recipe { get; set; }
    }
}
