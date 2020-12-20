using server.repository;
using Serilog;
using server.models;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using server.exceptions;

namespace server.user
{
    public class UserRepository : GenericRepository<User>
    {
        private ILogger log;
        public UserRepository(dwfContext context, ILogger log) : base(context){
            this.log = log;
            this.log.ForContext<UserRepository>();
        }

        public int CreateUserSalt(string salt) {
            User u = new User(){
                Salt = salt
            };
            context.Users.Add(u);
            context.SaveChanges();
            return u.Id;
        }

        public string GetUserSalt(string username) {
            User user = context.Users
                .Where(u => u.Username.Equals(username))
                .FirstOrDefault();
            if (user == null) throw new NotFoundException("User does not exist");
            return user.Salt.Trim();
        }

        public override User GetByID(object id) {
            
            return context.Users.Where(u => u.Id == (int)id).Select(u => new User(){
                Username = u.Username.Trim(),
                Email = u.Email.Trim(),
                Id = u.Id,
                DisplayName = u.DisplayName.Trim()
            }).SingleOrDefault();
        }
        public User SaveUser(JObject userInfo) {
            try {
                string token = (string) userInfo.SelectToken("token");
                // log.Information($"token: {token}");
                string email = (string) userInfo.SelectToken("email");
                // log.Information($"email: {email}");
                string username = (string) userInfo.SelectToken("username");
                // log.Information($"username: {username}");
                int userId = (int) userInfo.SelectToken("userId");
                // log.Information($"id: {userId}");
                
                User user = context.Users.Where(u => u.Id == userId).FirstOrDefault();
                user.Email = email;
                user.Password = token;
                user.Username = username;
                context.SaveChanges();
                return user;
            } 
            catch (Exception e) {
                log.Error(e.Message);
                log.Error(e.InnerException.ToString());
                log.Error(e.ToString());
                return null;
            }

        }

        public User CheckLogin(JObject userInfo) {
            try {
                string token = ((string) userInfo.SelectToken("token"));
                log.Debug($"token: {token}");
                int userId = (int) userInfo.SelectToken("userId");
                log.Debug($"usedId: {userId}");
                string username = ((string) userInfo.SelectToken("username"));
                log.Debug($"username: {username}");
                User user = context.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (user == null) {
                    throw new NotFoundException("User not found");
                }
                if (!(user.Username.Trim().Equals(username) && user.Id == userId && user.Password.Trim().Equals(token))) {
                    return null;
                }
                return this.GetByID(userId);
            } 
            catch (Exception e) {
                log.Error(e.Message);
                log.Error(e.InnerException.ToString());
                log.Error(e.ToString());
                return null;
            }
            
        }
        public User Login(JObject userInfo) {
            string token = ((string) userInfo.SelectToken("token"));
            log.Debug($"token: {token}");
            string username = (string) userInfo.SelectToken("username");
            log.Debug($"username: {username}");
            User user = context.Users.Where(u => u.Username.Trim().Equals(username)).FirstOrDefault();
            if (user == null) {
                throw new NotFoundException("User not found");
            }
            if (!user.Password.Trim().Equals(token)) {
                throw new NotAuthorizedException("Invalid login");
            }
            return this.GetByID(user.Id);
        }

        public User GetByUsername(string username) {
            User user = context.Users
                .Where(u => u.Username.Trim() == username)
                .Select(u => new User(){
                    Username = u.Username.Trim(),
                    Email = u.Email.Trim(),
                    Id = u.Id,
                    DisplayName = u.DisplayName.Trim()})
                .FirstOrDefault();
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }
    }
}