using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using System.Diagnostics;
using System.Security.Cryptography;
using CommonClass;
namespace ServerLib
{
    class UserController :ControllerBasic
    {
        public UserController():base(){

        }
        public override Dictionary<string, Func<HttpServerContext, string>> GetActions()
        {
            var actions = new Dictionary<string, Func<HttpServerContext, string>>();
            actions["checkLogin"] = checkLogin;
            actions["registe"] = registe;

            return actions;
        }

        public string checkLogin(HttpServerContext context)
        {
            try
            {
                var usermodel = new UserModel(Connection);
                var request_json = Json.Decode(context.Request) ?? throw new InvalidArguments("Json decode Faild");
                var user_name = request_json["username"] ?? throw new InvalidArguments("Json decode Faild");
                var pass_word = request_json["password"] ?? throw new InvalidArguments("Json decode Faild");
                var user_info = usermodel.CheckLogin(user_name, pass_word) ?? throw new InvalidRequest(1, "错误的账号名或密码");
                var tmp = new Json();
                tmp["user_info"] = Json.ConvertFrom(user_info);
                context.session.Data["user_id"] = user_info.UserID;
                context.Write(GenerateResponse(tmp).Encode());
                
            }
            catch(InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code,a.Message,null).Encode());
            }
            return null;

        }
        public string registe(HttpServerContext context)
        {
            try
            {
                var usermodel = new UserModel(Connection);
                var request_json = Json.Decode(context.Request) ?? throw new InvalidArguments("Json decode Faild");
                var user_name = request_json["username"] ?? throw new InvalidArguments("Json decode Faild");
                var pass_word = request_json["password"] ?? throw new InvalidArguments("Json decode Faild");
                var nickname = request_json["nickname"] ?? throw new InvalidArguments("Json decode Faild");

                var user_info = usermodel.GetUserInfoByUserName(user_name);
                if(user_info!=null) throw new InvalidRequest(1,"该用户名已注册");
                user_info = usermodel.NewUser(user_name, GetMD5(pass_word),nickname,"0.0.0.0");
                if(user_info==null || user_info.UserID<=0) throw new InvalidRequest(1, "注册失败！");
                var tmp = new Json();
                tmp["user_id"] = user_info.UserID;
                context.Write(GenerateResponse(tmp).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        
        private static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }

        
    }
}
