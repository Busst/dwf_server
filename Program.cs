using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Serilog;
using Newtonsoft.Json; 
using server.user;
using server.repository;
using server.exceptions;
using server.recipes;
using server.LogEnrichers;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithCaller()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message}                               (at {Caller}){NewLine}{Exception}")
                .CreateLogger();
            Log.Logger.ForContext<Program>();
            Log.Information("Logger initialized");
            FOSContext dbContext = new FOSContext();
            SimpleServerListener(new string[]{
                "http://localhost:7321/",
                },
                dbContext);
        }

        // This example requires the System and System.Net namespaces.
        public static void SimpleServerListener(string[] prefixes, FOSContext dbContext)
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
            Log.Information("Listening...");
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            string res = null;
            try {
                res = HandleRequest(request, dbContext);
            } catch (NotFoundException e) {
                res = e.Message;
            } catch (FormatException e) {
                res = e.Message;
            } catch (Exception e) {
                res = "Generic Exception caught: " + e.Message;
                Log.Error(e.StackTrace);
            }
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = $"<HTML><BODY>{res}</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer,0,buffer.Length);
            // You must close the output stream.
            output.Close();
            listener.Stop();
        }

        public static string HandleRequest(HttpListenerRequest req, FOSContext dbContext){
            string[] tempPath = req.RawUrl.Split("/");
            List<String> realPath = new List<string>();
            string s = null;
            if (tempPath.Length > 1) s = tempPath[1];
            if (tempPath.Length > 2) {
                for (int i = 0; i < tempPath.Length-2; i++){
                    realPath.Add(tempPath[i+2]);
                }
            }

            IRequestHandler controller = null;
            Log.Information("Switch: " + s);
            switch (s){
                case "test":
                    System.Console.WriteLine("test path");
                    break;
                case "user":
                    controller = new UserController(Log.Logger, dbContext);
                    break;
                case "drinks":
                    controller = new RecipesController(Log.Logger, dbContext);
                    break;
                case "cooking":
                    break;
                default:
                    break;
            } 
            if (controller == null) {
                throw new NotFoundException("404: File Path Not Found - First link");
            }
            string res = controller.HandleRequest(req);
            return $"<br/>{res}<br/>";
        }
    }
}
