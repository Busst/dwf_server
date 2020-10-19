using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace server.user
{
    public class UserContext : DbContext
    {
        public UserContext() : base("Server=DESKTOP-9SG1883\\DWF_SQL;Database=dwf;Trusted_Connection=True;Uid=tbuss;Pwd=Margaritaville")
        {
            
        }

        public DbSet<UserModel> Users {get; set;}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users");
            

        }
    }
}