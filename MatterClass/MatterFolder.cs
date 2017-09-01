using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
namespace CommonClass
{
    public class MatterFolder:IJsonAble
    {
        public int folder_id=0;
        public int user_id = 0;
        public string folder_name="";
        public string img_src = "";
        public MatterFolder() { }


        public Object fromJson(Json json)
        {
            return json.ConvertTo<MatterFolder>((string c, string m) =>
            {
                switch (m)
                {
                    case "img_src":
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
                }
                return true;
            });
        }
    }
}
