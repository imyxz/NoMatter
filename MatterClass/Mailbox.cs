using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
namespace CommonClass
{
    public class Mailbox : IJsonAble
    {
        public int mailbox_id = 0;
        public int user_id = 0;
        public string email_address="",email_password = "", pop3_address = "", end_uid = "";
        public int pop3_port = 0;
        public bool use_ssl = false;

        public Mailbox() { }

        public string Email_address { get => email_address; set => email_address = value; }

        public Object fromJson(Json json)
        {
            return json.ConvertTo<Mailbox>((string c, string m) =>
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
