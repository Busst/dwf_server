using server.repository;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using Serilog;
using server.models;
using Microsoft.EntityFrameworkCore;
using server.Resources;
using server.exceptions;

namespace server.recipes
{
    public class RecipeRepository : GenericRepository<Recipe>
    {
        ILogger log;
        public RecipeRepository(dwfContext context, ILogger log) : base(context){
            this.log = log;
            this.log.ForContext<RecipeRepository>();
        }

        public IEnumerable<Recipe> GetByType(string type) {
            
            // return from Recipe r in (List<Recipe>)this.Get()
            //         where r.Type == getType
            //         select r;
            return null;
        }

        public override Recipe GetByID(object id)
        {
            Recipe r = base.GetByID(id);
            r.Ingredients = context.Ingredients.Where(i => i.RecipeId == r.Id).ToList();
            return r;
        }

        public Recipe[] GetByUserID(object id)
        {
            User u = context.Users.Where(u => u.Id == (int) id).FirstOrDefault();
            Recipe[] recipes = context.Recipes
                .Where(r => r.UserId == (int) id)
                .Select(r => new Recipe(){
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Likes = r.Likes,
                    Type = r.Type,
                    Ingredients = r.Ingredients,
                    UserId = r.UserId,
                    User = new User(){
                        Username = u.Username
                    }
                })
                .ToArray();
            return recipes;
        }

        public Recipe GetByUserDrinkName(object o_creatorName, object o_name)
        {
            string id = (string) o_creatorName;
            string name = (string) o_name;
            log.Information($"Getting drink with user: {id} and name: {name}");
            User u = context.Users.Where(u => u.Username == id).FirstOrDefault();
            if (u == null) {
                throw new NotFoundException("404: User not found");
            }
            Recipe recipe = context.Recipes
                .Where(r => r.UserId == u.Id && r.Name.Equals(name))
                .Select(r => new Recipe(){
                    Id = r.Id,
                    Description = r.Description,
                    Name = r.Name,
                    Type = r.Type,
                    UserId = r.UserId,
                    Likes = r.Likes,
                    Background = r.Background ?? "",
                    User = new User(){
                        Username = u.Username,
                        DisplayName = (u.DisplayName ?? u.Username ?? "")
                    }
                })
                .FirstOrDefault();
            if (recipe == null) {
                throw new NotFoundException("404: Recipe not found");
            }
            return recipe;
        }

        public override void Update(Recipe entity)
        {   //redo this man 
            Recipe r = GetByID (entity.Id);
            
            foreach (Ingredient i in entity.Ingredients) {
                if (i.Id != 0) {
                    Ingredient old = context.Ingredients.Find(i.Id);
                    log.Information($"updating {old.Name} for Recipe {entity.Name} Id {entity.Id}");
                    old.Name = i.Name;
                    old.Measurement = i.Measurement;
                    old.Quantity = i.Quantity;
                } else {
                    log.Information($"Adding {i.Name} for Recipe {entity.Name} Id {entity.Id}");
                    Ingredient n = new Ingredient{
                        Name = i.Name,
                        Recipe = i.Recipe,
                        Quantity = i.Quantity,
                        Measurement = i.Measurement,
                        RecipeId = entity.Id,
                    };
                    context.Ingredients.Add(n);
                    context.SaveChanges();
                }
            }
            r.Description = entity.Description;
            r.Likes = entity.Likes;
            r.Name = entity.Name;
            r.Type = entity.Type;
            context.SaveChanges();
        }

        public override void Delete(object id) {
            Recipe r = GetByID(id);
            context.Recipes.Remove(r);
            context.SaveChanges();
        }
    }
}