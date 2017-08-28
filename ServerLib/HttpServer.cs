using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
namespace ServerLib
{
    public class HttpServer
    {
        private readonly HttpListener listener = new HttpListener();
        private Dictionary<string, Dictionary<string, Func<HttpServerContext, string>>> Handlers = new Dictionary<string, Dictionary<string, Func<HttpServerContext, string>>>();
        public HttpServer(params string [] prefixes)
        {
            foreach (string prefix in prefixes)
            {
                listener.Prefixes.Add(prefix);
            }
        }
        public void Start()
        {
            listener.Start();
            ThreadPool.QueueUserWorkItem(Runner, null);
        }
        public void Registe(string ControllerName,IController Controller)
        {
            Handlers[ControllerName] = Controller.GetActions();
        }
        private void Runner(object state)
        {
            while(listener.IsListening)
            {
                var context = new HttpServerContext();
                var ListenerContext = listener.GetContext();
                context.Context = ListenerContext;
                ThreadPool.QueueUserWorkItem(Handler, context);



            }
        }
        private void Handler(object state)
        {
            var context = (HttpServerContext)state;
            var ListenerContext = context.Context;
            try
            {
                string[] query = ListenerContext.Request.RawUrl.Split('/');
                if (query.Length < 3) throw new Exception("Not mvc");
                string controller = query[1];
                string action = query[2];
                if (!Handlers.ContainsKey(controller)) throw new Exception("No such controller");
                if (!Handlers[controller].ContainsKey(action)) throw new Exception("No such action");
                context.Controller = controller;
                context.Action = action;
                context.Handler = Handlers[controller][action];
                for (int index = 4; index < query.Length; index += 2)//填充Arguments
                {
                    context.Arguments[query[index - 1]] = query[index];
                }
                context.Handler(context);
                string responseStr = context.Response;
                byte[] buf = Encoding.UTF8.GetBytes(responseStr);
                ListenerContext.Response.ContentLength64 = buf.Length;
                ListenerContext.Response.OutputStream.Write(buf, 0, buf.Length);
            }
            catch(Exception a)
            {
                ListenerContext.Response.StatusCode = 500;
                Console.WriteLine("HttpServerWrong:" + a.ToString() + "\n" + ListenerContext.Request.RawUrl);
            }
            finally
            {

                ListenerContext.Response.OutputStream.Close();
            }
            
        }
        
    }
}
