using System;
using System.Collections.Generic;

#nullable disable

namespace server.models
{
    public partial class Recipe
    {
        public Recipe()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public RecipeType Type { get; set; } = RecipeType.Food;
        public int? Likes { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
