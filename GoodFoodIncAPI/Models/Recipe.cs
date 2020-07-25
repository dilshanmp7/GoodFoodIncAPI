﻿using System;
using System.Collections.Generic;

namespace GoodFoodIncAPI.Models
{
    public class Recipe
    {
        private string _title;
        private string _slug;

        public Recipe()
        {
            IngredientInfoes = new HashSet<IngredientInfo>();
        }

        public int RecipeId { get; set; }
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

        public int CatagoryId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

        //Navigation Properties
        public Catagory Catagory { get; set; }
        public User User { get; set; }
        public ICollection<IngredientInfo> IngredientInfoes { get; set; }
    }
}
