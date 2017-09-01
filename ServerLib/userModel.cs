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
        public UserInfo GetUserInfoByUserName(string username)
        {
            var result = QueryStmt("select * from user_info where user_name=@0 limit 1", username);
            if (result.Count > 0)
            {
                var info = result[0];
                var user_info = new UserInfo();
                user_info.UserID = Int32.Parse(info["user_id"]);
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
        public UserInfo GetUserInfoByUserID(int user_id)
        {
            var result = QueryStmt("select * from user_info where user_id=@0 limit 1", user_id.ToString());
            if (result.Count > 0)
            {
                var info = result[0];
                var user_info = new UserInfo();
                user_info.UserID = Int32.Parse(info["user_id"]);
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
        public UserInfo NewUser(string username,string password,string user_nickname,string ip)
        {
            var ret= new UserInfo();
            ret.UserName = username;
            ret.PassWord = password;
            ret.NickName = user_nickname;
            var result = QueryStmt(@"insert into user_info set user_name=@0,user_email="",user_avatar="",user_nickname=@1,user_password=@2,reg_ip=@3,login_ip=@3", ret.UserName,ret.NickName,ret.PassWord,ip);
            ret.UserID = (int)result.InsertID;
            return ret;

        }
        public void ChangePassword(int user_id,string password)
        {
            QueryStmt(@"update user_info set user_password=@0 where user_id=@1 limit 1", password,user_id.ToString());

        }
        

    }
}
