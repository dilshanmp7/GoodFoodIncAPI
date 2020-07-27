using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoodFoodIncAPI.Response
{
    public class RecipesRespondModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Dictionary<string,string> Ingredients { get; set; }
        public string Owner { get; set; }

        public RecipesRespondModel()
        {
            Ingredients = new Dictionary<string, string>();
        }
    }
}
