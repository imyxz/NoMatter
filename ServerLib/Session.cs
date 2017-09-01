using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using MySql.Data.MySqlClient;
namespace ServerLib
{
    public class Session:ModelBasic
    {
        private HttpServerContext context;
        private long sessionID;
        private string sessionPass;
        private static readonly int sessionTime = 60 * 24 * 30;
        private Json data = new Json();
        private readonly static string session_prefix = "nomatter_";
        public Json Data { get => data; set => data = value; }

        public Session(HttpServerContext context) :base(context.DBConnection)
        {
            this.context = context;
        }

        public void StartSession()
        {
            if(FindCookie(out sessionID,out sessionPass))
            {
                var session_info = GetSessionInfo(sessionID);
                if(session_info!=null && session_info["session_pass"]!="" )
                {
                    if(session_info["session_pass"] == sessionPass)
                    {
                        Data = Json.Decode(session_info["session_info"]);
                        return;
                    }

                }
                var cookie = new System.Net.Cookie(session_prefix+sessionID.ToString(),"empty","/");
                cookie.Expired = true;
                AddCookie(cookie);
            }
            NewSession();

        }
        private void NewSession(int session_minutes=-1)
        {
            if (session_minutes < 0)
                session_minutes = sessionTime;
            sessionPass = getRandomString(32);
            var result = QueryStmt("insert into session_info set session_pass=@0,session_info='',session_start_time=now(),session_end_time=DATE_ADD(now(),INTERVAL @1 MINUTE),session_last_time=now()," +
                "session_status=0", sessionPass, session_minutes.ToString());
            if (result.InsertID <= 0)
                throw new Exception("Create Session Faild!");
            sessionID = result.InsertID;
        }
        private string getRandomString(int length)
        {
            var rand = new Random();
            string ret = "";
            string str = "abcdefghijklmnopqrstuvwxyz1234567890";
            for (int i=0;i<length;i++)
            {
                ret +=str[rand.Next(0, str.Length)];
            }
            return ret;
        }
        private bool FindCookie(out long session_id,out string session_pass)
        {
            var cookies = context.Context.Request.Cookies;
            session_id = 0;
            session_pass = "";
            for(int i=0;i<cookies.Count;i++)
            {
                var cookie = cookies[i];
                if(cookie.Name.Length>session_prefix.Length && cookie.Name.Substring(0,session_prefix.Length)==session_prefix)
                {
                    string tmp = cookie.Name.Substring(session_prefix.Length);
                    if (!Int64.TryParse(tmp, out session_id)) continue;
                    session_pass = cookie.Value;
                    return true;
                }
            }
            return false;
        }
        private Json GetSessionInfo(long session_id)
        {
            var result = QueryStmt("select * from session_info where session_id=@0 and session_status=0 limit 1", session_id.ToString());
            if (result.Count > 0)
            {
                return Json.Import<string, string>(result[0]);
            }
            else
                return null;
        }
        public void SaveSession()
        {
            QueryStmt("update session_info set session_info=@0,session_end_time=DATE_ADD(now(),INTERVAL @1 MINUTE) " +
                "where session_id=@2", data.Encode() ?? "", sessionTime.ToString(), sessionID.ToString());
            var cookie = new System.Net.Cookie(session_prefix+sessionID.ToString(),sessionPass);
            cookie.Expires = DateTime.Now.AddMinutes(sessionTime);
            cookie.Path = "/";
            AddCookie(cookie);
        }
        private void AddCookie(System.Net.Cookie cookie)
        {
            var max_age=cookie.Expires.Subtract(DateTime.Now).Duration().TotalSeconds;
            context.Context.Response.AppendHeader("Set-Cookie", cookie.Name + "=" + cookie.Value + "; Max-Age=" + max_age + "; Path=" + cookie.Path);
        }

    }
}
