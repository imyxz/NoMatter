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
        private Dictionary<string, IController> Controllers = new Dictionary<string, IController>();
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
            Controllers[ControllerName] = Controller;
        }
        private void Runner(object state)
        {
            Log("服务器启动！监听于:" + listener.Prefixes.First());
            while (listener.IsListening)
            {
                var context = new HttpServerContext();//此处线程阻塞
                var ListenerContext = listener.GetContext();
                context.Context = ListenerContext;
                ThreadPool.QueueUserWorkItem(Handler, context);//启动新线程处理请求



            }
        }
        protected void Handler(object state)
        {
            var context = (HttpServerContext)state;
            var ListenerContext = context.Context;

            IController Controller=null;
            try
            {
                string[] query = ListenerContext.Request.RawUrl.Split('/');
                if (query.Length < 3) throw new Exception("Not mvc");
                string controller_name = query[1];
                string action = query[2];

                if (!Controllers.ContainsKey(controller_name)) throw new Exception("No such controller");
                Controller = Controllers[controller_name].InitController(Controllers[controller_name].GetType());//初始化控制器
                context.DBConnection = Controller.GetDBConnection();
                var Handlers = Controller.GetActions();//获取控制器方法
                if (!Handlers.ContainsKey(action)) throw new Exception("No such action");
                context.Controller = controller_name;
                context.Action = action;
                context.Handler = Handlers[action];
                byte[] requestBuf = new byte[ListenerContext.Request.ContentLength64];
                ListenerContext.Request.InputStream.Read(requestBuf, 0, (int)ListenerContext.Request.ContentLength64);
                context.Request = Encoding.UTF8.GetString(requestBuf);
                for (int index = 4; index < query.Length; index += 2)//填充Arguments
                {
                    context.Arguments[query[index - 1]] = query[index];
                }
                Log(ListenerContext.Request.UserHostAddress + "连入 " + controller_name + " " + action);
                Log(context.Request);
                context.session = new Session(context);
                context.session.StartSession();//session调用
                context.Handler(context);//调用方法
                context.session.SaveSession();//保存session调用信息
                Log(context.Response);

                string responseStr = context.Response;
                byte[] buf = Encoding.UTF8.GetBytes(responseStr);
                ListenerContext.Response.ContentLength64 = buf.Length;
                ListenerContext.Response.OutputStream.Write(buf, 0, buf.Length);

            }
            catch (Exception a)
            {
                ListenerContext.Response.StatusCode = 500;
                Log("HttpServerWrong:" + a.ToString() + "\n" + ListenerContext.Request.RawUrl);
            }
            finally
            {
                if(Controller != null)
                {
                    try { Controller.EndController(); } catch { }//结束控制器
                }
                ListenerContext.Response.OutputStream.Close();
            }
            
        }
        private void Log(string msg)
        {
            Console.WriteLine(msg);
        }

    }
    
}
