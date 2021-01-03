using server.repository;
using Serilog;
using server.models;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using server.exceptions;

namespace server.user
{
    public class UserLikesRepository : GenericRepository<UserLikesRecipes> {
        private ILogger log;
        public UserLikesRepository(dwfContext context, ILogger log) : base(context){
            this.log = log;
            this.log.ForContext<UserLikesRecipes>();
        }

        public bool IsLiked(int userId, int recipeId){
            return context.UserLikesRecipes.Where(u => u.UserId == userId && u.RecipeId == recipeId).FirstOrDefault() != null;
        }

        public int LikeRecipe(int userId, int recipeId){
            try {
                UserLikesRecipes ulr = new UserLikesRecipes{
                    RecipeId = recipeId,
                    UserId = userId
                };
                context.UserLikesRecipes.Add(ulr);
                context.SaveChanges();
                return ulr.Id;
            } catch {
                return -1;
            }
        }
        public bool UnlikeRecipe(int userId, int recipeId){
            try {
                UserLikesRecipes ulr = context.UserLikesRecipes.Where(u => u.UserId == userId && u.RecipeId == recipeId).FirstOrDefault();
                if (ulr == null){
                    return false;
                }
                context.UserLikesRecipes.Remove(ulr);
                context.SaveChanges();
            } catch {
                return false;
            }
            return true;
        }
    }


}