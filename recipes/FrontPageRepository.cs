using server.repository;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using Serilog;
using server.models;
using Microsoft.EntityFrameworkCore;
using server.Resources;
using server.exceptions;

namespace server.recipes
{
    public class FrontPageRepository : GenericRepository<Recipe>
    {
        ILogger log;
        public FrontPageRepository(dwfContext context, ILogger log) : base(context){
            this.log = log;
            this.log.ForContext<FrontPageRepository>();
        }
        public string GetCarouselItems(){
            int[] rids = context.FrontPage
                        .Where(e => e.Type == FrontPageType.Drink || e.Type == FrontPageType.Food)
                        .Select(e => e.RecipeId)
                        .ToArray();
            string[] notes = context.FrontPage
                        .Where(e => e.Type == FrontPageType.Notes)
                        .Select(e => e.Notes)
                        .ToArray();
            List<object> frontPage = new List<object>();
            foreach (int id in rids) {
                frontPage.Add(context.Recipes.Find(id));
            }
            frontPage.Add(notes);
            return Parsing.ParseList(frontPage);
        }

        public void AddNotes(FrontPage page) {
            context.FrontPage.Add(page);
            context.SaveChanges();
        }

        public void AddDaily(int first, int second){
            FrontPage[] rids = context.FrontPage
                        .Where(e => e.Type == FrontPageType.Drink || e.Type == FrontPageType.Food)
                        .ToArray();
            if (rids.Length > 0){
                rids[0].RecipeId = first;
            } else {
                context.FrontPage.Add(new FrontPage{
                    RecipeId = first
                });
            }
            if (rids.Length > 1){
                rids[1].RecipeId = second;
            } else {
                context.FrontPage.Add(new FrontPage{
                    RecipeId = second
                });
            }
            context.SaveChanges();
        }

    }
}