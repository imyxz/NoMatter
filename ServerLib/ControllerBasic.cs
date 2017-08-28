using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace ServerLib
{
    public class ControllerBasic
    {
        protected MySqlConnection Connection;
        private static string connnect_flag = @"server=localhost;userid=user12;
            password=34klq*;database=mydb";
        public ControllerBasic()
        {
            Connection = new MySqlConnection(connnect_flag);
        }
    }
}
