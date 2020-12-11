using System.Collections.Generic;
using server.repository;
using Serilog;
using server.models;

namespace server.recipes.ingredients
{
    public class IngredientRepository : GenericRepository<Ingredient>
    {
        private ILogger log;
        public IngredientRepository(dwfContext context, ILogger log) : base(context){
            this.log = log;
            this.log.ForContext<IngredientRepository>();
        }

        
        
    }
}