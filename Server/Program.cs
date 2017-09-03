using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLib;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            MatterServer server = new MatterServer();
            server.Start("http://127.0.0.1:9090/");
            Console.ReadKey();
        }
    }
}
