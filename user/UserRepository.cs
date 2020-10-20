using server.repository;

namespace server.user
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(FOSContext context) : base(context){
            
        }
    }
}