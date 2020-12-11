using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using Serilog;
using Newtonsoft.Json; 
using server.user;
using server.repository;
using server.exceptions;
using server.recipes;
using server.LogEnrichers;
using server.Resources;
using Newtonsoft.Json.Linq;
using server.models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace server
{
    class Program
    {
        private static bool runOnce = false;
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithCaller()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message}                               (at {Caller}){NewLine}{Exception}")
                .MinimumLevel.Debug()
                .CreateLogger();
            Log.Logger.ForContext<Program>();
            Log.Information("Logger initialized");
            
            ServerConfig serverConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"./ServerConfig.json"));

            dwfContext dbContext = new dwfContext(serverConfig);
            SimpleServerListener(new string[]{
                "http://localhost:7321/",
                },
                dbContext);
        }

        // This example requires the System and System.Net namespaces.
        public static void SimpleServerListener(string[] prefixes, dwfContext dbContext)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine ("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            do {
                Log.Information($"Run Once Enabled {runOnce}");
                Log.Information("Listening...");
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                JObject body = Parsing.ParseRequestData(request);
                Log.Debug($"url: {request.Url}");
                Log.Debug($"data: {body}");
                string res = null;
                int status = 200;
                try {
                    res = HandleRequest(request, dbContext, body);
                } catch (NotAuthorizedException e) {
                    res = e.Message;
                    status = 401;
                } catch (NotFoundException e) {
                    res = e.Message;
                    status = 404;
                } catch (FormatException e) {
                    res = e.Message;
                    status = 400;
                } catch (Exception e) {
                    status = 500;
                    res = "Generic Exception caught: " + e.Message;
                    Log.Error($"response: {res}");
                    Log.Error($"message: {e.Message}");
                    Log.Error($"stacktrace: {e.StackTrace}");
                    Log.Error($"inner: {e.InnerException}");
                    Log.Error($"target method: {e.TargetSite}");
                }
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                if (request.HttpMethod == "OPTIONS")
                {
                    response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                    response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                    response.AddHeader("Access-Control-Max-Age", "1728000");
                }
                response.AppendHeader("Access-Control-Allow-Origin", "*");
                // Construct a response.
                response.StatusCode = status;
                string responseString = $"{res}";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            } while (!runOnce);
            // listener.Stop();
        }

        public static string HandleRequest(HttpListenerRequest req, DbContext dbContext, JObject body){
            string[] tempPath = req.RawUrl.Split("/");
            string s = null;
            if (tempPath.Length > 1) s = tempPath[1];
            
            IRequestHandler controller = null;
            Log.Debug("First Subdirectory: " + s);
            switch (s){
                case "users":
                    controller = new UserController(Log.Logger, dbContext);
                    break;
                case "recipes":
                    controller = new RecipesController(Log.Logger, dbContext);
                    break;
                default:
                    break;
            } 
            if (controller == null) {
                throw new NotFoundException($"404: File Path Not Found - First link\r\n{s} does not match\r\nusers\r\nrecipes");
            }
            string res = controller.HandleRequest(req, body);
            return $"{res}";
        }
    }
}
