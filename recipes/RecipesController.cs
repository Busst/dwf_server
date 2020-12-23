using System;
using System.Net;
using Serilog;
using server.exceptions;
using System.Collections.Specialized;
using System.Collections.Generic;
using server.repository;
using server.Resources;
using server.models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace server.recipes
{
    public class RecipesController : HttpHandler, IRequestHandler
    {
        // private st/ring reponse;
        private ILogger log;
        private UnitOfWork unitOfWork;
        private HttpListenerResponse httpListenerResponse;
        public RecipesController(ILogger log, ServerConfig serverConfig, HttpListenerResponse response) {
            this.log = log;
            this.log.ForContext<RecipesController>();
            this.log.Information("Creating a new user repo");
            unitOfWork = new UnitOfWork(this.log,  new dwfContext(serverConfig));
            this.httpListenerResponse = response;
        }
        public RecipesController(ILogger log, DbContext dbContext, HttpListenerResponse response) {
            this.log = log;
            this.log.ForContext<RecipesController>();
            unitOfWork = new UnitOfWork(this.log, (dwfContext) dbContext);
            this.httpListenerResponse = response;
        }
        public override void HandleGet(string[] segments, NameValueCollection queries, string hash, string nextPath) {
            log.Information("Next Path: " + nextPath);
            //Directories should follow convention {directory} but WHY/
            switch (nextPath.ToLower()) {
                case("getall"):
                    // response = Parsing.ParseList(unitOfWork.RecipeRepository.Get());
                    break;
                case("getbyid"):
                    if (queries["id"] == null) throw new Exception("404");
                    response = Parsing.ParseObject(unitOfWork.RecipeRepository.GetByID(Int32.Parse(queries["id"])));
                    break;
                case("getbytype"):
                    response = Parsing.ParseObject(unitOfWork.RecipeRepository.GetByType("drink"));
                    break;
                case("getingredbyid"):
                    response = Parsing.ParseObject(unitOfWork.IngredientRepository.GetByID(1));
                    break;
                case("getdrinksbyuserid"):
                    response = Parsing.ParseList(unitOfWork.RecipeRepository.GetByUserID(Int32.Parse(queries["id"])));
                    break;
                case("getbycreatorname"):
                    response = Parsing.ParseObject(unitOfWork.RecipeRepository.GetByUserDrinkName(queries["creator"], queries["name"]));
                    break;
                case("search"):
                    response = SearchDrinks(queries["input"]);
                    break;
                case("deeper/"):
                    nextPath = Parsing.ParseSegment(segments, out segments);
                    HandleGet(segments, queries, hash, nextPath);
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Second link");
            }
        }
        public override void HandlePost(string[] segments, NameValueCollection queries, string hash, string nextPath){
            Recipe r;
            switch (nextPath.ToLower()) {
                case("savedrink"):
                    r = body.ToObject<Recipe>();
                    unitOfWork.RecipeRepository.Insert(r);
                    response = Parsing.ParseObject(r);
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Second link");
            }
            unitOfWork.Save();
        }
        public override void HandlePatch(string[] segments, NameValueCollection queries, string hash, string nextPath){
            switch (nextPath.ToLower()) {
                case("update"):
                    Recipe r = body.ToObject<Recipe>();
                    unitOfWork.RecipeRepository.Update(r);
                    response = Parsing.ParseObject(r);
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Second link");
            }
        }
        public override void HandleDelete(string[] segments, NameValueCollection queries, string hash, string nextPath){
            
            switch (nextPath.ToLower()) {
                case("deletebyid"):
                    if (queries["id"] == null) throw new Exception("404");
                    Recipe u = unitOfWork.RecipeRepository.GetByID(Int32.Parse(queries["id"]));
                    log.Information($"{u}");
                    unitOfWork.RecipeRepository.Delete(Int32.Parse(queries["id"]));
                    unitOfWork.Save();
                    break;

                default:
                    throw new NotFoundException("File Path Not Found: Second link");
            }
        }
        public override void HandlePut(string[] segments, NameValueCollection queries, string hash, string nextPath){

        }

        public string SearchDrinks(string param) {
            Ingredient[] ingredients = unitOfWork.IngredientRepository.Search(param);
            Recipe[] recipes = unitOfWork.RecipeRepository.Search(param);
            Dictionary<int, int> drinks = new Dictionary<int, int>();
            foreach(Ingredient i in ingredients) {
                if (drinks.ContainsKey(i.RecipeId)){
                    drinks[i.RecipeId]++;
                } else {
                    drinks[i.RecipeId] = 1;
                }
            }
            foreach(Recipe r in recipes) {
                if (drinks.ContainsKey(r.Id)){
                    drinks[r.Id]++;
                } else {
                    drinks[r.Id] = 1;
                }
            }
            List<KeyValuePair<int, int>> list = drinks.ToList();
            list.Sort((x, y) => (x.Value - y.Value));
            return Parsing.ParseList(list);
        }
        
        
    }
}