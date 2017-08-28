using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
namespace ServerLib
{
    class UserController :ControllerBasic, IController
    {
        public UserController():base(){

        }
        public Dictionary<string, Func<HttpServerContext, string>> GetActions()
        {
            var actions = new Dictionary<string, Func<HttpServerContext, string>>();
            actions["test"] = test;
            return actions;
        }
        public string test(HttpServerContext context)
        {
            context.Write("Fuck!");
            context.WriteLine("Fuck Again!");
            return null;
        }
        public string checkLogin(HttpServerContext context)
        {
            var usermodel = new UserModel(Connection);
            var request_json = Json.Decode(context.Request) ?? throw new InvalidArguments("Json decode Faild");
            var user_name=request_json["username"] ?? throw new InvalidArguments("Json decode Faild");
            var pass_word=request_json["password"] ?? throw new InvalidArguments("Json decode Faild");
            var user_id=usermodel.CheckLogin(user_name, pass_word);

        }


    }
}
