using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class ClientConfig
    {
        private static string serverUrl = "http://127.0.0.1:9090/";

        public static string ServerUrl { get => serverUrl; private set => serverUrl = value; }
    }
}
