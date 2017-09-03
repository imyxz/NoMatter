using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Pop3;
using OpenPop;
using CommonClass;
namespace ServerLib
{
    static class EmailReceiver
    {
        public static List<MatterEmail> GetEmails(string email,string password,string pop3_address,int pop3_port,bool use_ssl,string end_uid,int count=20)
        {
            Pop3Client client = new Pop3Client();
            
            client.Connect(pop3_address, pop3_port, use_ssl);
            client.Authenticate(email, password);
            int messageCount = client.GetMessageCount();
            var ret = new List<MatterEmail>();
            for (int i = 0; i<count && i<messageCount; i++)
            {
                try
                {
                    var tmp = client.GetMessage(messageCount - i);
                    var uid = client.GetMessageUid(messageCount - i);
                    if (uid == end_uid)
                        break;
                    var matter_email = new MatterEmail();
                    var mail = tmp.ToMailMessage();
                    matter_email.matter_name = mail.Subject;
                    var to_email = mail.To[0].Address ?? "";
                    var body = ReplaceHtmlTag(mail.Body, mail.Body.Length);
                    if (body.Length >= 200)
                        body = body.Substring(0, 200);
                    matter_email.matter_desc = "From: "+ mail.From+"\n To: "+ to_email + "\n"+ body;

                    matter_email.matter_end_time = DateTime.Parse(tmp.Headers.Date);
                    matter_email.matter_next_effect_time = matter_email.matter_start_time = matter_email.matter_end_time;
                    matter_email.email_uid = uid;
                    ret.Add(matter_email);
                }
                catch {  }
            }
            client.Disconnect();
            client.Dispose();
            return ret;
        }
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }
}
