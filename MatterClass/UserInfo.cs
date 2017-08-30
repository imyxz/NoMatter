using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;
namespace CommonClass
{
    public class UserInfo:IJsonAble
    {
        private int userID;
        private string userName;
        private string passWord;
        private string nickName;

        public int UserID { get => userID; set => userID = value; }
        public string UserName { get => userName; set => userName = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public string NickName { get => nickName; set => nickName = value; }

        public void fromJson(Json json)
        {
            throw new NotImplementedException();
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