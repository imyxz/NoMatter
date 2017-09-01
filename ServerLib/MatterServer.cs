using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ServerLib
{
    public class MatterServer
    {
        static Dictionary<string,IController> Controllers = new Dictionary<string, IController>();
        private HttpServer http;
        static MatterServer()
        {
            Controllers["user"]=new UserController();
            Controllers["matter"] = new MatterController();
        }
        public void Start(params string [] prefixes)
        {
            http = new HttpServer(prefixes);
            foreach(var pair in Controllers)
            {
                http.Registe(pair.Key, pair.Value);
            }
            http.Start();
        }
    }
}
