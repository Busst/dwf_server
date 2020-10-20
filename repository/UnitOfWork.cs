using System;
using server.recipes;
using server.recipes.ingredients;
using server.user;
using Serilog;
namespace server.repository
{
    public class UnitOfWork : IDisposable
    {
        private FOSContext context;
        private UserRepository userRepository;
        private RecipeRepository recipeRepository;
        private IngredientRepository ingredientRepository;
        private ILogger log;
        public UnitOfWork(ILogger logger, FOSContext context){
            this.context = context;
            log = logger;
            log.ForContext<UnitOfWork>();
        }
        public UserRepository UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(context);
                }
                return userRepository;
            }
        }

        public RecipeRepository RecipeRepository
        {
            get
            {

                if (this.recipeRepository == null)
                {
                    this.recipeRepository = new RecipeRepository(context);
                }
                return recipeRepository;
            }
        }
        public IngredientRepository IngredientRepository
        {
            get
            {

                if (this.ingredientRepository == null)
                {
                    this.ingredientRepository = new IngredientRepository(context);
                }
                return ingredientRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}