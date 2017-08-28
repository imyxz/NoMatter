using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;

namespace ServerLib
{
    class UserModel : ModelBasic
    {
        public UserModel(MySqlConnection connect) : base(connect)
        {
        }
        public int CheckLogin(string username,string password)
        {
            var result = QueryStmt("select user_id from user_info where user_name=@0 and user_password=@1 limit 1",  username, password);
            if(result.Count>0)
            {
                return Int32.Parse(result[0]["user_id"]);
            }
            else
            {
                return 0;
            }
        }
    }
}
