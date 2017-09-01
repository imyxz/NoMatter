using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HttpRequest;
using JSON;
using CommonClass;
namespace Client
{
    static class ClientSession
    {
        private static Cookie sessionCookie = new Cookie();

        public static Cookie SessionCookie { get => sessionCookie; set => sessionCookie = value; }
        public static List<MatterBasic> matters = new List<MatterBasic>();
        public static List<MatterFolder> folders = new List<MatterFolder>();
        public static UserInfo user_info = new UserInfo();
        public static Func<bool> OnMattersChange;
        public static Func<bool> OnUserInfoChange;
        public static Func<bool> OnFoldersChange;
        public static MatterBasic cur_selected_matter;
        public static bool is_cur_selected_matter_changed = false;
        public static bool ReloadMatters()
        {
            bool flag = false;
            cur_selected_matter = null;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var result = Json.Decode(query.Get(ClientConfig.ServerUrl + "matter/getMatters"));
                if (!result.IsNull() && result["status"] == 0)
                {
                    var lists = result["info"]["matters"];
                    matters.Clear();
                    foreach (var tmp in lists)
                    {
                        matters.Add(MatterBasic.ParseFromJson(tmp.Value));
                    }
                    flag = true;
                }
            }
            catch { }
            ClientSession.is_cur_selected_matter_changed = false;
            if (flag && OnMattersChange != null)
                OnMattersChange();

            return flag;
            
        }
        public static bool SaveMatter(MatterBasic matter)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var requestJson = Json.ConvertFrom<MatterBasic>(matter);
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "matter/saveMatter",requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadMatters();
                }
            }
            catch { }
            return flag;

        }
        public static bool ReloadFolders()
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var result = Json.Decode(query.Get(ClientConfig.ServerUrl + "matter/getFolders"));
                if (!result.IsNull() && result["status"] == 0)
                {
                    var lists = result["info"]["folders"];
                    folders.Clear();
                    foreach (var tmp in lists)
                    {
                        folders.Add(tmp.Value.ConvertTo<MatterFolder>());
                    }
                    flag = true;
                }
            }
            catch { }
            if (flag && OnFoldersChange != null)
                OnFoldersChange();

            return flag;

        }
    }

}
