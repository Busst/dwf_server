using System.Net;
using System;
using System.Collections.Specialized;

namespace server
{
    public class HttpHandler : IRequestHandler
    {
        protected string response {get; set;}
        public virtual string HandleRequest(HttpListenerRequest req){
            
            switch(req.HttpMethod) {
                case "GET":
                    HandleGet(req.Url.Segments, req.QueryString, req.Url.Fragment);
                    break;
                case "POST":
                    HandlePost(req.Url.Segments, req.QueryString, req.Url.Fragment);
                    break;
                case "PATCH":
                    HandlePatch(req.Url.Segments, req.QueryString, req.Url.Fragment);
                    break;
                case "DELETE":
                    HandleDelete(req.Url.Segments, req.QueryString, req.Url.Fragment);
                    break;
                case "PUT":
                    HandlePut(req.Url.Segments, req.QueryString, req.Url.Fragment);
                    break;
                default:
                    throw new Exception("Request Error");
            }
            return response;
        }
        public virtual void HandleGet(string[] segments, NameValueCollection queries, string hash) {
            
        }
        public virtual void HandlePost(string[] segments, NameValueCollection queries, string hash){

        }
        public virtual void HandlePatch(string[] segments, NameValueCollection queries, string hash){

        }
        public virtual void HandleDelete(string[] segments, NameValueCollection queries, string hash){

        }
        public virtual void HandlePut(string[] segments, NameValueCollection queries, string hash){

        }
    }
}