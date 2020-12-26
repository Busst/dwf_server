using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System;
using server.models;

namespace server.Resources
{
    public class Parsing
    {
        public static string ParseList(IEnumerable<User> list){
            return JsonConvert.SerializeObject(list, Formatting.Indented, 
                    new JsonSerializerSettings(){
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
        }

        public static string ParseList(List<KeyValuePair<int, int>> list){
            return JsonConvert.SerializeObject(list, Formatting.Indented, 
                    new JsonSerializerSettings(){
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
        }
        public static string ParseList(List<object> list){
            return JsonConvert.SerializeObject(list, Formatting.Indented, 
                    new JsonSerializerSettings(){
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
        }
        public static string ParseList(IEnumerable<Recipe> list){
            return JsonConvert.SerializeObject(list, Formatting.Indented, 
                    new JsonSerializerSettings(){
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
        }
        public static string ParseList(IEnumerable<Ingredient> list){
            return JsonConvert.SerializeObject(list, Formatting.Indented, 
                    new JsonSerializerSettings(){
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
        }
        public static string ParseObject(object o){
            return JsonConvert.SerializeObject(JObject.FromObject(o, new JsonSerializer(){
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }), Formatting.Indented);
        }
        public static JObject ParseString(object o){
            return JObject.Parse((string) o);
        }

        public static string ParseSegment(string[] splitPath, out string[] segments){
            string nextPath = "";
            if (splitPath.Length > 1){
                nextPath = splitPath[1];
            }
            segments = new string[splitPath.Length-1];
            segments[0] = splitPath[0];
            for (int i = 1; i < splitPath.Length-1; i++)
            {
                segments[i] = splitPath[i+1];
            }
            return nextPath;
        }

        public static JObject ParseRequestData(HttpListenerRequest request){
            if (!request.HasEntityBody)
            {
                return null;
            }
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            string s = reader.ReadToEnd();
            
            JObject json = JObject.Parse(s);
            body.Close();
            return json;
        }
        public static Ingredient TrimIngredient(Ingredient i){
            i.Measurement = i.Measurement.Trim();
            i.Quantity = i.Quantity.Trim();
            i.Name = i.Name.Trim();
            return i;
        }
        public static Ingredient[] TrimIngredients(Ingredient[] ingredients){
            List<Ingredient> listIngreds = new List<Ingredient>();
            foreach (Ingredient i in ingredients) {
                listIngreds.Add(TrimIngredient(i));
            }
            return listIngreds.ToArray();
        }
        public static Ingredient[] TrimIngredients(ICollection<Ingredient> ingredients){
            List<Ingredient> listIngreds = new List<Ingredient>();
            foreach (Ingredient i in ingredients) {
                listIngreds.Add(TrimIngredient(i));
            }
            return listIngreds.ToArray();
        }        
    }
}