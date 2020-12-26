namespace server.models
{
    public class FrontPage
    {
        public int Id {get; set;}
        public FrontPageType Type {get; set;} = FrontPageType.Notes;
        public int RecipeId {get; set;}
        public string Notes {get; set;}
    }
}