using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace server
{
    public interface IRequestHandler
    {
        string HandleRequest(HttpListenerRequest req, JObject body);
        void HandleGet(string[] segments, NameValueCollection queries, string hash, string nextPath);
        void HandlePost(string[] segments, NameValueCollection queries, string hash, string nextPath);
        void HandlePatch(string[] segments, NameValueCollection queries, string hash, string nextPath);
        void HandleDelete(string[] segments, NameValueCollection queries, string hash, string nextPath);
        void HandlePut(string[] segments, NameValueCollection queries, string hash, string nextPath);
    } 
    
}