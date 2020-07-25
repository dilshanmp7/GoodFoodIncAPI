using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public partial class Ingredient
    {

        private string _title;
        private string _slug;

        public int IngredientId { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                _slug = $"{UserId}-{_title}";
            }
        }
        public string Slug
        {
            get => _slug;
        }
        public string Description { get; set; }
        public int UserId { get; set; }

        public Ingredient()
        {
            IngredientInfoes = new HashSet<IngredientInfo>();
        }

        //Navigation properties
        public User User { get; set; }
        public ICollection<IngredientInfo> IngredientInfoes { get; set; }
    }
}
