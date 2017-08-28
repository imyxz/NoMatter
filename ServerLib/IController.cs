using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public interface IController
    {
        Dictionary<string, Func<HttpServerContext, string>> GetActions();
    }
}
