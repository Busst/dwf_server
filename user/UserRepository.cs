using server.repository;
using Serilog;
using server.models;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using server.exceptions;
using server.Resources.Enums;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;

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

        public string GetUserSalt(string email) {
            User user = context.Users
                .Where(u => u.Email == email)
                .FirstOrDefault();
            if (user == null) throw new NotFoundException("User does not exist");
            return user.Salt;
        }

        public string GetByUsernameByEmail(string email) {
            User user = context.Users
                .Where(u => u.Email == email)
                .FirstOrDefault();
            return user.Username;
        }

        public void UpdateUser(User updatedUser, int userId) {
            var user = context.Users.Single(u => u.Id == userId);
            
            log.Information($"user id: {user.Id}");
            if (updatedUser.DisplayName != null) {
                log.Information($"Display Name changing from {user.DisplayName ?? "NULL"} to {updatedUser.DisplayName}");
                user.DisplayName = updatedUser.DisplayName;
            }
            context.SaveChanges();
        }

        public override User GetByID(object id) {
            
            return context.Users.Where(u => u.Id == (int)id).Select(u => new User(){
                Username = u.Username,
                Email = u.Email,
                Id = u.Id,
                DisplayName = u.DisplayName
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

        public User CheckLogin(string token, int id) {
            User user = context.Users.Where(u => 
                    u.AccessToken == token &&
                    u.Id == id)
                .Select(u => new User{
                    DisplayName = u.DisplayName,
                    Username = u.Username,
                    Id = u.Id
                })
                .FirstOrDefault();
            return user;
        }

        public bool Logout(int id) {
            try {
                User user = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if (user == null) return true;
                user.AccessToken = "";
                context.SaveChanges();
            } catch (Exception e) {
                log.Debug(e.Message);
            }
            return true;
        }
        public User Login(JObject userInfo) {
            string token = ((string) userInfo.SelectToken("token"));
            // log.Debug($"token: {token}");
            string email = (string) userInfo.SelectToken("email");
            // log.Debug($"email: {email}");
            User user = context.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user == null) {
                throw new NotFoundException("User not found");
            }
            log.Debug($"{user.Password} == {token}");
            if (!(user.Password == token)) {
                throw new NotAuthorizedException("Invalid login");
            }
            log.Information($"User: {user.Username} has logged in succesfully");
            return user;
        }

        public User GetByUsername(string username) {
            User user = context.Users
                .Where(u => u.Username == username)
                .Select(u => new User(){
                    Username = u.Username,
                    Email = u.Email,
                    Id = u.Id,
                    DisplayName = u.DisplayName})
                .FirstOrDefault();
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }

        public void SaveAccessToken(int id, string accessToken) {
            User user = context.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            user.AccessToken = accessToken;
            user.LastLogin = DateTime.Now;
            context.SaveChanges();
        }

        public bool SaveImage(Picture imageDTO) {
            try {
                
                imageDTO.Image.Save(imageDTO.FilePath, ImageFormat.Jpeg);
            } catch (Exception e) {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                return false;
            }
            
            return true;
        }

        public Image GetImage(string filePath, string fileName) {
            Image image = Image.FromFile($"{filePath}/{fileName}.jpeg");
            
            return image;
        }

        public long GetFileSize(string filePath) {
            FileInfo file = new FileInfo(filePath);
            return file.Length;
        }

        public bool LikeDrink(){

            return false;
        }
        public bool UnlikeDrink(){
            return false;
        }
        
    }
}