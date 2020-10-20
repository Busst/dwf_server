using server.repository;

namespace server.recipes
{
    public class RecipeRepository : GenericRepository<Recipe>
    {
        public RecipeRepository(FOSContext context) : base(context){
            
        }
    }
}