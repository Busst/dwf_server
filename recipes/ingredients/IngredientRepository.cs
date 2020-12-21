using server.repository;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using Serilog;
using server.models;
using Microsoft.EntityFrameworkCore;
using server.Resources;
using server.exceptions;

namespace server.recipes.ingredients
{
    public class IngredientRepository : GenericRepository<Ingredient>
    {
        private ILogger log;
        public IngredientRepository(dwfContext context, ILogger log) : base(context){
            this.log = log;
            this.log.ForContext<IngredientRepository>();
        }

        
        public Ingredient[] Search(string param) {
            IEnumerable<Ingredient> ingredients = context.Ingredients.Where(i => 
                i.Name.Contains(param)
                || i.Measurement.Contains(param)
                || i.Quantity.Contains(param))
                
                .Select(i => new Ingredient{
                    RecipeId = i.RecipeId
            });
            return ingredients.ToArray();
        }
    }
}