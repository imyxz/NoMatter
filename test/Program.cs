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
            /*a["status"] = true;
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
            a = Json.Decode(tmp);*/
            string json_string = "";
            /*json_string = System.IO.File.ReadAllText(@"D:\test.txt");
            a = Json.Decode(json_string);
            a = Json.Decode(json_string);
            a = Json.Decode(json_string);
            a = Json.Decode(json_string);*/
            test tester = new test();
            a = Json.ConvertFrom(tester);
            foreach(KeyValuePair<string,Json> pair in a)
            {
                Console.WriteLine(pair.Key + " " + pair.Value.Encode());
            }
            Console.WriteLine(a.Encode());
            Console.ReadKey();
        }
    }
    class test
    {
        public string a, b, c, d;
        public test1 datetime = new test1();
        private string ed;
        public test()
        {
            a = "123";
            b = "456";
            c = "789";
        }
        public string xxx()
        {
            return "";
        }
    }
    class test1
    {
        public DateTime a;
        public test1()
        {
            a = new DateTime();
        }
    }
}
