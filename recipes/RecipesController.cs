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
using Newtonsoft.Json.Linq;

namespace server.recipes
{
    public class RecipesController : HttpHandler, IRequestHandler
    {
        // private st/ring reponse;
        private ILogger log;
        private UnitOfWork unitOfWork;
        public RecipesController(ILogger log, ServerConfig serverConfig) {
            this.log = log;
            this.log.ForContext<RecipesController>();
            this.log.Information("Creating a new user repo");
            unitOfWork = new UnitOfWork(this.log,  new dwfContext(serverConfig));
        }
        public RecipesController(ILogger log, DbContext dbContext) {
            this.log = log;
            this.log.ForContext<RecipesController>();
            unitOfWork = new UnitOfWork(this.log, (dwfContext) dbContext);
            
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
        
        
    }
}