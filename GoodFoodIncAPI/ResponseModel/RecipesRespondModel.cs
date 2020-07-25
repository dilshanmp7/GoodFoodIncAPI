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
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<KeyValuePair<string,string>> Ingredients { get; set; }
        public string Owner { get; set; }

        public RecipesRespondModel()
        {
            Ingredients = new List<KeyValuePair<string, string>>();
        }
    }
}
