using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;
using CommonClass;
namespace ServerLib
{
    class UserModel : ModelBasic
    {
        public UserModel(MySqlConnection connect) : base(connect)
        {
        }
        public UserInfo CheckLogin(string username,string password)
        {
            var result = QueryStmt("select * from user_info where user_name=@0 and user_password=@1 limit 1",  username, password);
            if(result.Count>0)
            {
                var info = result[0];
                var user_info = new UserInfo();
                user_info.UserID =Int32.Parse(info["user_id"]);
                user_info.UserName = info["user_name"];
                user_info.NickName = info["user_nickname"];
                user_info.PassWord = info["user_password"];
                return user_info;

            }
            else
            {
                return null;
            }
        }
        
    }
}
