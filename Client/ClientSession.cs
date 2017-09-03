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
        public static List<Mailbox> mailboxes = new List<Mailbox>();

        public static UserInfo user_info = new UserInfo();
        public static Func<bool> OnMattersChange;
        public static Func<bool> OnUserInfoChange;
        public static Func<bool> OnFoldersChange;
        public static Func<bool> OnMailBoxChange;

        public static MatterBasic cur_selected_matter;
        public static int cur_selected_matter_pos=0;
        public static bool is_cur_selected_matter_changed = false;
        public static int FindFolderIndex(int folder_id,List<MatterFolder> folders)
        {
           for(var i=0;i<folders.Count;i++)
            {
                if (folder_id == folders[i].folder_id)
                    return i+1;
            }
            return 0;
        }
        public static bool ReloadMatters()
        {
            bool flag = false;
            cur_selected_matter = null;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var response = query.Get(ClientConfig.ServerUrl + "matter/getMatters");
                var result = Json.Decode(response);
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
            if (flag && OnFoldersChange != null)
                OnFoldersChange();

            return flag;
            
        }
        public static bool ReloadMailboxs()
        {
            bool flag = false;
            cur_selected_matter = null;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var response = query.Get(ClientConfig.ServerUrl + "mailbox/getMailboxs");
                var result = Json.Decode(response);
                if (!result.IsNull() && result["status"] == 0)
                {
                    var lists = result["info"]["mailboxs"];
                    mailboxes.Clear();
                    foreach (var tmp in lists)
                    {
                        mailboxes.Add(tmp.Value.ConvertTo<Mailbox>());
                    }
                    flag = true;
                }
            }
            catch { }
            ClientSession.is_cur_selected_matter_changed = false;
            if (flag && OnMailBoxChange != null)
                OnMailBoxChange();

            return flag;

        }
        public static bool SaveMatter(MatterBasic matter)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var tmp = matter;
                if(matter.GetType()!=typeof(MatterEmail))
                {
                    if (matter.matter_addion_info["period_type"].IsNull() || matter.matter_addion_info["period_type"] == MatterPeriodicityType.OneTime.ToString())
                    {
                        tmp = MatterOneOff.converToMe(matter);
                    }
                    else
                    {
                        tmp = MatterPeriodicity.converToMe(matter, (MatterPeriodicityType)Enum.Parse(typeof(MatterPeriodicityType), (string)matter.matter_addion_info["period_type"]));

                    }
                }
                
                var requestJson = Json.ConvertFrom<MatterBasic>(tmp);
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "matter/saveMatter",requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadMatters();
                }
            }
            catch { }
            return flag;

        }
        public static bool SaveMailBox(Mailbox mailbox)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);

                var requestJson = Json.ConvertFrom<Mailbox>(mailbox);
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "mailbox/saveMailbox", requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadMailboxs();
                }
            }
            catch { }
            return flag;

        }
        public static bool DeleteMailBox(int mailbox_id)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);

                var requestJson = new Json();
                requestJson["mailbox_id"] = mailbox_id;
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "mailbox/deleteMailbox", requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadMailboxs();
                }
            }
            catch { }
            return flag;

        }
        public static bool DeleteMatter(int matter_id)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var requestJson = new Json();
                requestJson["matter_id"] = matter_id;
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "matter/deleteMatter", requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadMatters();
                }
            }
            catch { }
            return flag;

        }
        public static bool FinishMatter(int matter_id)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var requestJson = new Json();
                requestJson["matter_id"] = matter_id;
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "matter/finishMatter", requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadMatters();
                }
            }
            catch { }
            return flag;

        }
        public static bool DeleteFolder(int folder_id)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var requestJson = new Json();
                requestJson["folder_id"] = folder_id;
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "matter/deleteFolder", requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadFolders();

                }
            }
            catch { }
            return flag;

        }
        public static bool AddFolder(string folder_name)
        {
            bool flag = false;
            try
            {
                HttpQuery query = new HttpQuery();
                query.RequestCookies.Add(ClientSession.SessionCookie);
                var requestJson = new Json();
                requestJson["folder_name"] = folder_name;
                var result = Json.Decode(query.Post(ClientConfig.ServerUrl + "matter/addFolder", requestJson.Encode()));
                if (!result.IsNull() && result["status"] == 0)
                {
                    ReloadFolders();
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
