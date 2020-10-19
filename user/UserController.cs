using System.Net;
using Serilog;
using System.Collections.Generic;
using System.Collections.Specialized;
using server.repository;

namespace server.user
{
    public class UserController : HttpHandler, IRequestHandler
    {
        // private st/ring reponse;
        private ILogger log;
        private UserRepository userRepository;
        public UserController(ILogger log) {
            this.log = log;
            this.log.ForContext<UserController>();
            this.log.Information("Creating a new user repo");
            userRepository = new UserRepository(this.log, new UserContext());
        }
        public UserController(ILogger log, IRepository<UserModel> repo) {
            this.log = log;
            this.log.ForContext<UserController>();
            userRepository = (UserRepository) repo;
            
        }

        public override string HandleRequest(HttpListenerRequest req){
            return base.HandleRequest(req);
        }
        public override void HandleGet(string[] segments, NameValueCollection queries, string hash) {
            // response = $"{userRepository.GetUsers()}";
            log.Information("Handling Get in user controller");
            List<UserModel> users = (List<UserModel>)userRepository.GetUsers();
            log.Information($"{users.Capacity}");
            foreach(UserModel user in users) {
                log.Information($"User id: {user.Id} : {user.Username}");
            }
            response = "set a response in users";
        }
        public override void HandlePost(string[] segments, NameValueCollection queries, string hash){

        }
        public override void HandlePatch(string[] segments, NameValueCollection queries, string hash){

        }
        public override void HandleDelete(string[] segments, NameValueCollection queries, string hash){

        }
        public override void HandlePut(string[] segments, NameValueCollection queries, string hash){

        }
        
        
    }
}