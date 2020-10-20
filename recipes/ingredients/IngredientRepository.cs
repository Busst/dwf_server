using server.repository;

namespace server.recipes.ingredients
{
    public class IngredientRepository : GenericRepository<Recipe>
    {
        public IngredientRepository(FOSContext context) : base(context){
            
        }
    }
}