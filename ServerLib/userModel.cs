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
                user_info.user_email = info["user_email"];
                user_info.user_phone = info["user_phone"];

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
                user_info.user_email = info["user_email"];
                user_info.user_phone = info["user_phone"];
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
                user_info.user_email = info["user_email"];
                user_info.user_phone = info["user_phone"];
                return user_info;

            }
            else
            {
                return null;
            }
        }
        public int NewUser(string username,string password,string user_nickname,string user_email,string user_phone)
        {
            var result = QueryStmt(@"insert into user_info set user_name=@0 , user_avatar='',user_nickname= @1 ,user_password=@2,reg_ip='',login_ip='' , user_email=@3 , user_phone=@4 ", 
                username,user_nickname,password,user_email,user_phone);
            return (int)result.InsertID;

        }
        public void ChangePassword(int user_id,string password)
        {
            QueryStmt(@"update user_info set user_password=@0 where user_id=@1 limit 1", password,user_id.ToString());

        }
        

    }
}
