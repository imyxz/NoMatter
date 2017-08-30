using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using System.Diagnostics;
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

        public override IController InitController()
        {
            var tmp= new UserController();
            tmp.ConnectDB();
            return tmp;
        }
    }
}
