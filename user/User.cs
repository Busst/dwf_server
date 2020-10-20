using server.repository;

namespace server.user
{
    public class User : IEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}