using Newtonsoft.Json;

namespace server.Resources
{
    public class ServerConfig
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public string Trusted { get; set; }
    }
}