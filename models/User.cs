using System;
using System.Collections.Generic;

#nullable disable

namespace server.models
{
    public partial class User
    {
        public User()
        {
            Recipes = new HashSet<Recipe>();
        }
        public string Username { get ; set; }
        public int Id { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
