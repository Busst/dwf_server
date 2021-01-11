using System.Net;
using System;
using System.Collections.Specialized;
using server.Resources;
using Newtonsoft.Json.Linq;

namespace server
{
    public class HttpHandler : IRequestHandler
    {
        protected HttpListenerRequest request { get; set; }
        protected string response {get; set;}
        public virtual string HandleRequest(HttpListenerRequest req){
            request = req;
            string nextPath = Parsing.ParseSegment(req.Url.Segments, out string[] segments);
            nextPath = Parsing.ParseSegment(segments, out segments);
            switch(req.HttpMethod) {
                case "GET":
                    HandleGet(segments, req.QueryString, req.Url.Fragment, nextPath);
                    break;
                case "POST":
                    HandlePost(segments, req.QueryString, req.Url.Fragment, nextPath);
                    break;
                case "PATCH":
                    HandlePatch(segments, req.QueryString, req.Url.Fragment, nextPath);
                    break;
                case "DELETE":
                    HandleDelete(segments, req.QueryString, req.Url.Fragment, nextPath);
                    break;
                case "PUT":
                    HandlePut(segments, req.QueryString, req.Url.Fragment, nextPath);
                    break;
                default:
                    throw new Exception("Request Error");
            }
            return response;
        }
        public virtual void HandleGet(string[] segments, NameValueCollection queries, string hash, string nextPath) {
            
        }
        public virtual void HandlePost(string[] segments, NameValueCollection queries, string hash, string nextPath){

        }
        public virtual void HandlePatch(string[] segments, NameValueCollection queries, string hash, string nextPath){

        }
        public virtual void HandleDelete(string[] segments, NameValueCollection queries, string hash, string nextPath){

        }
        public virtual void HandlePut(string[] segments, NameValueCollection queries, string hash, string nextPath){

        }
    }
}