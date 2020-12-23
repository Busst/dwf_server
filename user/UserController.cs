using System;
using System.Net;
using System.Web;
using Serilog;
using server.exceptions;
using System.Collections.Specialized;
using server.repository;
using System.Security.Cryptography;
using server.Resources;
using server.models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;

namespace server.user
{
    public class UserController : HttpHandler, IRequestHandler
    {
        // private st/ring reponse;
        private ILogger log;
        private UnitOfWork unitOfWork;
        private HttpListenerResponse httpListenerResponse;
        public UserController(ILogger log, ServerConfig serverConfig, HttpListenerResponse response) {
            this.log = log;
            this.log.ForContext<UserController>();
            unitOfWork = new UnitOfWork(this.log, new dwfContext(serverConfig));
            this.httpListenerResponse = response;
        }
        public UserController(ILogger log, DbContext dbContext, HttpListenerResponse response) {
            this.log = log;
            this.log.ForContext<UserController>();
            unitOfWork = new UnitOfWork(this.log, (dwfContext) dbContext);
            this.httpListenerResponse = response;
        }
        public override void HandleGet(string[] segments, NameValueCollection queries, string hash, string nextPath) {
            
            //Directories should follow convention {directory}/
            switch (nextPath.ToLower()) {
                case("getsalt"):
                    response = new JObject(
                        new JProperty("salt", unitOfWork.UserRepository.GetUserSalt(queries["username"])),
                        new JProperty("username", unitOfWork.UserRepository.GetByUsernameByEmail(queries["username"]))
                    ).ToString();
                    break;
                case("createsalt"):
                    string salt = GenerateSalt();
                    var userId = unitOfWork.UserRepository.CreateUserSalt(salt);
                    response = new JObject(
                        new JProperty("userId", userId),
                        new JProperty("salt", salt)
                    ).ToString();
                    break;
                case("getuserbyid"):
                    if (queries["id"] == null) throw new Exception("404");
                        response = Parsing.ParseObject(unitOfWork.UserRepository.GetByID(Int32.Parse(queries["id"])));
                    break;
                case("getuserbyusername"):
                    if (queries["id"] == null) throw new Exception("404");
                        response = Parsing.ParseObject(unitOfWork.UserRepository.GetByUsername(queries["id"]));
                    break;
                case(""):
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Get: Second link");
            }
        }
        public override void HandlePost(string[] segments, NameValueCollection queries, string hash, string nextPath){
            log.Information("Next Path " + nextPath);
            //Directories should follow convention {directory} but WHY/
            User user = null;
            string accessToken = null;
            switch (nextPath.ToLower()) {
                case("saveuser"):
                    user = unitOfWork.UserRepository.SaveUser(body);
                    if (user == null) throw new NotAuthorizedException("unable to save user");
                    accessToken = GetAccessToken(user.Password);
                    JObject package = new JObject(
                        new JProperty("user", Parsing.ParseString(Parsing.ParseObject(user))),
                        new JProperty("accessToken", accessToken)
                    );
                    unitOfWork.UserRepository.SaveAccessToken(user.Id, accessToken);
                    response = package.ToString();
                    break;
                case("checklogin"):
                    user = unitOfWork.UserRepository.CheckLogin(body);
                    if (user != null){
                        response = Parsing.ParseObject(user);
                    } else {
                        throw new NotAuthorizedException("Invalid login token");
                    }
                    break;
                case("login"):
                    user = unitOfWork.UserRepository.Login(body);
                    if (user != null){
                        accessToken = GetAccessToken(user.Password);
                        unitOfWork.UserRepository.SaveAccessToken(user.Id, accessToken);
                        Cookie cookie = new Cookie{
                            Name = "AccessToken",
                            Secure = true,
                            HttpOnly = true,
                            Value = HttpUtility.UrlEncode(accessToken)
                        };
                        httpListenerResponse.AppendCookie(cookie);
                        response = Parsing.ParseObject(user);
                    }
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Post: Second link");
            }
        }
        public override void HandlePatch(string[] segments, NameValueCollection queries, string hash, string nextPath){

        }
        public override void HandleDelete(string[] segments, NameValueCollection queries, string hash, string nextPath){
            
            switch (nextPath.ToLower()) {
                case("deletebyid"):
                if (queries["id"] == null) throw new Exception("404");
                    unitOfWork.UserRepository.Delete(Int32.Parse(queries["id"]));
                    break;
                default:
                    throw new NotFoundException("File Path Not Found: Delete: Second link");
            }
        }
        public override void HandlePut(string[] segments, NameValueCollection queries, string hash, string nextPath){

        }
        

        public string GenerateSalt() {
            byte[] salt = new byte[32];
            using (RNGCryptoServiceProvider rndCsp = new RNGCryptoServiceProvider()){
                rndCsp.GetBytes(salt);
            }
            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider()){
                string salt_string = Encoding.Default.GetString(sha256.ComputeHash(salt));
                log.Information($"Generated salt {salt_string}");
                return salt_string;
            }
        }
        public string GetAccessToken(string password) {
            byte[] bytes = Encoding.ASCII.GetBytes(password + DateTime.Now.ToString());  
            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider()){
                string token = Encoding.ASCII.GetString(sha256.ComputeHash(bytes));
                log.Information($"Generated token {token}");
                return token;
            }
        }
        
    }
}