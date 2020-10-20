using System;
using System.Net;
using Serilog;
using server.exceptions;
using System.Collections.Specialized;
using server.repository;
using server.Resources;

namespace server.recipes.ingredients
{
    public class IngredientController //: HttpHandler, IRequestHandler
    {
        private ILogger log;
        private UnitOfWork unitOfWork;
        public IngredientController(ILogger log) {
            this.log = log;
            this.log.ForContext<IngredientController>();
            this.log.Information("Creating a new user repo");
            unitOfWork = new UnitOfWork(this.log, new FOSContext());
        }
        public IngredientController(ILogger log, FOSContext dbContext) {
            this.log = log;
            this.log.ForContext<IngredientController>();
            unitOfWork = new UnitOfWork(this.log, dbContext);
            
        }

        // public override string HandleRequest(HttpListenerRequest req){
        //     return base.HandleRequest(req);
        // }
        // public override void HandleGet(string[] segments, NameValueCollection queries, string hash) {
            
        //     string nextPath = "";
        //     if (segments.Length > 2) {
        //         nextPath = segments[2];
        //     }
        //     switch (nextPath.ToLower()) {
        //         case("getall"):
        //             response = Parsing.ParseList(IngredientsRepository.GetIngredients());
        //             break;
        //         case("getbyid"):
        //             if (queries["id"] == null) throw new Exception("404");
        //                 response = Parsing.ParseObject(IngredientsRepository.GetByID(Int32.Parse(queries["id"])));
        //                 break;
                
        //         default:
        //             throw new NotFoundException("File Path Not Found: Second link");
        //     }
        // }
        // public override void HandlePost(string[] segments, NameValueCollection queries, string hash){

        // }
        // public override void HandlePatch(string[] segments, NameValueCollection queries, string hash){

        // }
        // public override void HandleDelete(string[] segments, NameValueCollection queries, string hash){
        //     string nextPath = "";
        //     if (segments.Length > 2) {
        //         nextPath = segments[2];
        //     }
        //     switch (nextPath.ToLower()) {
        //         case("deletebyid"):
        //             if (queries["id"] == null) throw new Exception("404");
        //                 IngredientsRepository.Delete(Int32.Parse(queries["id"]));
        //                 break;

        //         default:
        //             throw new NotFoundException("File Path Not Found: Second link");
        //     }
        // }
        // public override void HandlePut(string[] segments, NameValueCollection queries, string hash){

        // }
        
        
    }
}