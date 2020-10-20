using System;
using System.Net;
using Serilog;
using server.exceptions;
using System.Collections.Specialized;
using server.repository;

using server.Resources;

namespace server.user
{
    public class UserController : HttpHandler, IRequestHandler
    {
        // private st/ring reponse;
        private ILogger log;
        private UnitOfWork unitOfWork;
        public UserController(ILogger log) {
            this.log = log;
            this.log.ForContext<UserController>();
            this.log.Information("Creating a new user repo");
            unitOfWork = new UnitOfWork(this.log, new FOSContext());
        }
        public UserController(ILogger log, FOSContext dbContext) {
            this.log = log;
            this.log.ForContext<UserController>();
            unitOfWork = new UnitOfWork(this.log, dbContext);
            
        }

        public override string HandleRequest(HttpListenerRequest req){
            return base.HandleRequest(req);
        }
        public override void HandleGet(string[] segments, NameValueCollection queries, string hash) {
            
            string nextPath = "";
            if (segments.Length > 2) {
                nextPath = segments[2];
            }
            switch (nextPath.ToLower()) {
                // case("getall"):
                //     response = Parsing.ParseList(unitOfWork.UserRepository.GetAll());
                //     break;
                case("getbyid"):
                if (queries["id"] == null) throw new Exception("404");
                    response = Parsing.ParseObject(unitOfWork.UserRepository.GetByID(Int32.Parse(queries["id"])));
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Second link");
            }
        }
        public override void HandlePost(string[] segments, NameValueCollection queries, string hash){

        }
        public override void HandlePatch(string[] segments, NameValueCollection queries, string hash){

        }
        public override void HandleDelete(string[] segments, NameValueCollection queries, string hash){
            
            string nextPath = "";
            if (segments.Length > 2) {
                nextPath = segments[2];
            }
            switch (nextPath.ToLower()) {
                case("deletebyid"):
                if (queries["id"] == null) throw new Exception("404");
                    unitOfWork.UserRepository.Delete(Int32.Parse(queries["id"]));
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Second link");
            }
        }
        public override void HandlePut(string[] segments, NameValueCollection queries, string hash){

        }
        
        
    }
}