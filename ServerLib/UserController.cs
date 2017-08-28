using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    class UserController : IController
    {
        public Dictionary<string, Func<HttpServerContext, string>> GetActions()
        {
            var actions = new Dictionary<string, Func<HttpServerContext, string>>();
            actions["test"] = test;
            return actions;
        }
        public string test(HttpServerContext context)
        {
            context.Write("Fuck!");
            context.WriteLine("Fuck Again!");
            return null;
        }
    }
}
