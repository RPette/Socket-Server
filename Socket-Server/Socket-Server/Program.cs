using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Socket_Server
{
    internal class Program
    {
        public static HttpListener listener1;
        public static string url = "http://127.0.0.1:80/";
        public static string pageData = 
            "<!DOCTYPE html>" +
            "<html lang=\"en\">" +
            "<head>   " +
            "        <meta charset=\"UTF-8\">\r\n   " +
            "        <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    " +
            "        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n   " +
            "        <link rel=\"stylesheet\" href=\"styles.css\">" +
            "</head>" +
            "<body>    " +
            "        <h1>Ciao a tutti, belli e brutti :)</h1>\r\n" +
            "</body>" +
            "</html>";

        public static Task HandleIncomingConnections()
        {
            
            HttpListenerContext ctx = listener1.GetContext();

            // Peel out the requests and response objects
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData));
            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            resp.OutputStream.Write(data, 0, data.Length);

            return Task.CompletedTask;
        }

        static void Main(string[] args)
        {
            /*int porta_nota = 8000;
            Console.WriteLine("Starting Server (Listening on port " + porta_nota + ")");
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, porta_nota);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint); //associamo il socket TCP alla coppia <ip, porta_nota>

            Console.WriteLine("Waiting for a connection...");

            listener.Listen(100);// Server in ascolto in attesa di connessioni

            Socket handler = listener.Accept();

            Console.WriteLine("New incoming connection!");

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();*/

            listener1 = new HttpListener();
            listener1.Prefixes.Add(url);
            listener1.Start();
            Console.WriteLine("Server in Ascolo su : ", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener1.Close();

            
        }
    }
}
