using server.repository;
using System.Data.Entity;
using server.recipes.ingredients;
namespace server.recipes
{
    public class Recipe : IEntity
    {
        public DbSet<Ingredient> ingredients {get; private set;}
    }
}