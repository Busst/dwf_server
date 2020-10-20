using server.user;
using server.recipes;
using server.recipes.ingredients;
using System.Data.Entity;

namespace server.repository
{
    public class FOSContext : DbContext
    {
        public FOSContext() : base("Server=DESKTOP-3EOPPHT\\DWF_SQL;Database=dwf;Trusted_Connection=True;Uid=tbuss;Pwd=Margaritaville")
        {
            
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Recipe> Recipes {get; set;}
        public DbSet<Ingredient> Ingredients {get; set;}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Recipe>().ToTable("Recipes");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients");
        }
    }
}