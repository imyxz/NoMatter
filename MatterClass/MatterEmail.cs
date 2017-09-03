using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON;
namespace CommonClass
{
    public class MatterEmail : MatterBasic
    {
        public string email_uid = "";
        public MatterEmail():base()
        {
            matter_type = MatterType.email;
        }
        public override Object fromJson(Json json)
        {
            var tmp= json.ConvertTo<MatterEmail>((string c, string m) =>
            {
                switch (m)
                {
                    case "is_new":
                        return false;
                }
                return true;
            });
            tmp.email_uid = tmp.matter_addion_info["email_uid"];
            if (tmp.email_uid == null)
                tmp.email_uid = "";
            return tmp;
        }
        public override Json toJson()
        {
            this.matter_addion_info["email_uid"] = email_uid;
            return Json.ConvertFrom(this, (string c, string m) =>
            {
                switch (m)
                {
                    case "is_new":
                        return false;
                }
                return true;
            });
        }

        public override DateTime GetNextEffectTime()
        {
            throw new NotImplementedException();
        }
        public override bool OnUserFinish()
        {
            return true;
        }
    }
}