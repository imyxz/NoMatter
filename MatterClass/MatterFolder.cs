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
        public int folder_id = 0;
        public int user_id = 0;
        public string folder_name = "";
        public string img_src = "";
        public int matter_cnt = 0;
        public int Folder_id { get => folder_id; set => folder_id = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public string Folder_name { get => folder_name; set => folder_name = value; }
        public string Img_src { get => "Resources/"+img_src+"_black.png"; set => img_src = value; }
        public int Matter_cnt { get => matter_cnt; set => matter_cnt = value; }
        public string Visible { get => folder_id<=0?"Hidden":"Visible";  }
        public string Visible_mail { get => folder_name!="邮件" ? "Hidden" : "Visible"; }
        public int Width_mail { get => folder_name != "邮件" ? 0 : 24; }
        public MatterFolder() { }


        public Object fromJson(Json json)
        {
            return json.ConvertTo<MatterFolder>((string c, string m) =>
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
