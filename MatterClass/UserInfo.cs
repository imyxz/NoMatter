using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;
namespace CommonClass
{
    public class UserInfo:IJsonAble
    {
        public int UserID;
        public string UserName;
        public string PassWord;
        public string NickName;
        public string user_email = "";
        public string user_phone = "";
        public UserInfo() { }


        public Object fromJson(Json json)
        {
            return json.ConvertTo<UserInfo>((string c, string m) =>
            {
                switch (m)
                {
                    case "PassWord":
                        return false;
                }
                return true;
            });
        }

        public Json toJson()
        {
            return Json.ConvertFrom(this, (string c, string m) =>
            {
                switch (m)
                {
                    case "PassWord":
                        return false;
                }
                return true;
            });
        }
    }
}