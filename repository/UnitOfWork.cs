using System;
using server.recipes;
using server.recipes.ingredients;
using server.user;
using Serilog;
using server.models;

namespace server.repository
{
    public class UnitOfWork : IDisposable
    {
        private dwfContext context;
        private UserRepository userRepository;
        private RecipeRepository recipeRepository;
        private IngredientRepository ingredientRepository;
        private FrontPageRepository frontPageRepository;
        private ILogger log;
        public UnitOfWork(ILogger logger, dwfContext context){
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
                    this.userRepository = new UserRepository(context, log);
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
                    this.recipeRepository = new RecipeRepository(context, log);
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
                    this.ingredientRepository = new IngredientRepository(context, log);
                    
                }
                return ingredientRepository;
            }
        }

        public FrontPageRepository FrontPageRepository
        {
            get
            {

                if (this.frontPageRepository == null)
                {
                    this.frontPageRepository = new FrontPageRepository(context, log);
                    
                }
                return frontPageRepository;
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