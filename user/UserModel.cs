using server.repository;

namespace server.user
{
    public class UserModel : IEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}