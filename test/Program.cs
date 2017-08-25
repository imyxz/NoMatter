using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Json a=new Json();
            a["status"] = true;
            a["info"]["name"] = "董\"\\建华";
            a["info"]["old"] = 11;
            a["info"]["birth"] = 1988;
            a["info"]["money"] = 1.1;
            List<int> list = new List<int>();
            for(int i=0;i<5;i++)
            {
                list.Add(i);
            }
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary["123"] = 4456;
            dictionary["21312312"] = 3564;
            a["test"]["tset"] = Json.Import<int>(list);
            a["test"]["tse334t"] = Json.Import<string,int>(dictionary);
            string tmp = a.Encode();
            Console.WriteLine(tmp);
            a = Json.Decode(tmp);
            Console.WriteLine(a.Encode());
            Console.ReadKey();
        }
    }
}
