using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
namespace CommonClass
{
    public class Message : IJsonAble
    {
        public int message_id = 0,user_id=0;
        public bool is_sended_email = false, is_sended_sms = false, is_sended_client = false;
        public string message_title = "", message_body = "";
        public DateTime create_time = DateTime.Now;

        public Message() { }


        public Object fromJson(Json json)
        {
            return json.ConvertTo<Message>((string c, string m) =>
            {
                switch (m)
                {
                    case "img_src":
                        return false;
                    case "matter_cnt":
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
                    case "img_src":
                        return false;
                    case "matter_cnt":
                        return false;
                }
                return true;
            });
        }
    }
}
