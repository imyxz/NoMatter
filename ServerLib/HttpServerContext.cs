using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace ServerLib
{
    public class HttpServerContext
    {
        public Dictionary<string, string> Arguments = new Dictionary<string, string>();
        public HttpListenerContext Context;
        public string Controller;
        public string Action;
        public string Response = "";
        public Func<HttpServerContext, string> Handler;
        public void Write(string a)
        {
            Response += a;
        }
        public void WriteLine(string a)
        {
            Response += a + "\n";
        }
    }
}
