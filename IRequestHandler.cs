using System.Net;
using System.Collections.Specialized;
namespace server
{
    public interface IRequestHandler
    {
        string HandleRequest(HttpListenerRequest req);
        void HandleGet(string[] segments, NameValueCollection queries, string hash);
        void HandlePost(string[] segments, NameValueCollection queries, string hash);
        void HandlePatch(string[] segments, NameValueCollection queries, string hash);
        void HandleDelete(string[] segments, NameValueCollection queries, string hash);
        void HandlePut(string[] segments, NameValueCollection queries, string hash);
    } 
    
}