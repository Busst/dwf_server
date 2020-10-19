using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Serilog;
using server.user;


namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger.ForContext<Program>();
            Log.Information("Logger initialized");
            
            UserRepository userRepository = new UserRepository(Log.Logger, new UserContext());
            
            Log.Information($"{userRepository.GetUsers()}");
            SimpleServerListener(new string[]{
                "http://localhost:7321/"
            });
        }

        // This example requires the System and System.Net namespaces.
        public static void SimpleServerListener(string[] prefixes)
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
            
            string res = HandleRequest(request);
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

        public static string HandleRequest(HttpListenerRequest req){
            string[] path = req.RawUrl.Split("/");
            IRequestHandler controller = null;
            switch (path[1]){
                case "test":
                    System.Console.WriteLine("test path");
                    break;
                case "user":
                    controller = new UserController(Log.Logger);
                    break;
                default:
                    break;
            } 
            if (controller == null) {
                throw new Exception("Invalid file path: 404");
            }
            string res = controller.HandleRequest(req);
            return $"<br/>{res}<br/>";
        }
    }
}
