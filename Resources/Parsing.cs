using System.Collections.Generic;
using server.user;
using Newtonsoft.Json;
using server.recipes;
namespace server.Resources
{
    public class Parsing
    {
        public static string ParseList(IEnumerable<User> list){
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }
        public static string ParseList(IEnumerable<Recipe> list){
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }
        public static string ParseObject(object o){
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }
    }
}