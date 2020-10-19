using server.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Serilog;
namespace server.user
{
    public class UserRepository : IRepository<UserModel>
    {
        private UserContext context;
        private ILogger log;
        public UserRepository(ILogger log, UserContext context){
            this.log = log;
            this.log.ForContext<UserRepository>();
            this.log.Information("Adding Users");
            this.context = context;
            
            this.log.Information("User Repo instantiated");
        }

        public IEnumerable<UserModel> GetUsers(){
            return context.Users;
        }
        public void Delete(UserModel entityToDelete)
        {

        }
        public void Delete(object id)
        {

        }
        public IEnumerable<UserModel> Get(
            Expression<Func<UserModel, bool>> filter = null, 
            Func<IQueryable<UserModel>, IOrderedQueryable<UserModel>> orderBy = null, 
            string includeProperties = "")
        {
            return null;
        }
        public UserModel GetByID(object id){

            return null;
        }
        public IEnumerable<UserModel> GetWithRawSql(string query, 
            params object[] parameters)
        {
            return null;
        }
        public void Insert(UserModel entity)
        {

        }
        public void Update(UserModel entityToUpdate)
        {

        }
    }
}