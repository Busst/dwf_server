namespace server.models
{
    public class UserLikesRecipes
    {
        public int Id {get; set;}
        public int UserId {get; set;}
        public int RecipeId {get; set;}
    }
}