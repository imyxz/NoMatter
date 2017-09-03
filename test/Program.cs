using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using ServerLib;
namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Json json = new Json();
            json["anonymous"] = Json.ConvertFrom(new
            {
                name = "abc",
                song = "\"ewfaewfe\"",
                pic = 123,
                sub_obj = new
                {
                    name = "1234",
                    sub_obj = new
                    {
                        name = 123124
                    }
                }
            });
            Console.WriteLine(json.Encode());


            Console.ReadKey();



        }
    }
    class UserInfo
    {
        public string user_name="";
        public string pass_word="";
        public UserFriend friend=new UserFriend();
        public UserInfo() {  }
        public UserInfo(string name,string password,string friend) {
            user_name = name;
            pass_word = password;
            this.friend.friend_name = friend;
        }
    }
    class UserFriend
    {
        public string friend_name="";
    }
}
