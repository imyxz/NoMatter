using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using JSON;
namespace ServerLib
{
    abstract class ControllerBasic:IController
    {
        protected MySqlConnection Connection;
        private static string connnect_flag = @"server=localhost;userid=root;
            password=;database=nomatter;convert zero datetime=True;Convert Zero Datetime=True;Charset=utf8;";
        public ControllerBasic()
        {
            
        }
        public MySqlConnection ConnectDB()
        {
            Connection = new MySqlConnection(connnect_flag);
            Connection.Open();
            return Connection;
            
        }
        public void CloseConnect()
        {
            Connection.Close();
        }

        public abstract Dictionary<string, Func<HttpServerContext, string>> GetActions();

        public IController InitController(Type Controller)
        {
            
            var tmp = (ControllerBasic)Activator.CreateInstance(Controller);
            tmp.ConnectDB();
            return tmp;
        } 

        public void EndController()
        {
            CloseConnect();
        }
        public static Json GenerateResponse(Json a)
        {
            return GenerateResponse(0, "", a);
        }
        public static Json GenerateResponse(int status,string error,Json a)
        {
            var ret = new Json();
            ret["status"] = status;
            ret["error"] = error??"";
            ret["info"] = a??new Json();
            return ret;
        }

        public MySqlConnection GetDBConnection()
        {
            return Connection;
        }
    }
}
