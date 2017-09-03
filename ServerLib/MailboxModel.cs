using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;
using CommonClass;
using JSON;
namespace ServerLib
{
    class MailboxModel : ModelBasic
    {
        public MailboxModel(MySqlConnection connect) : base(connect)
        {
        }
        public int NewMailbox(Mailbox mailbox)
        {
            var result = QueryStmt(@"insert into user_mailbox set user_id=@0,email_address=@1,email_password=@2," +
                "pop3_address=@3,end_uid=@4,pop3_port=@5,use_ssl=@6",
                mailbox.user_id.ToString(),
                mailbox.Email_address,
                mailbox.email_password,
                mailbox.pop3_address,
                mailbox.end_uid,
                mailbox.pop3_port.ToString(),
                mailbox.use_ssl.ToString());
            return (int)result.InsertID;
        }
        public int SaveMailbox(Mailbox mailbox)
        {
            var result = QueryStmt(@"update user_mailbox set user_id=@0,email_address=@1,email_password=@2," +
                "pop3_address=@3,end_uid=@4,pop3_port=@5,use_ssl=@6 where mailbox_id=@7 limit 1",
                mailbox.user_id.ToString(),
                mailbox.Email_address,
                mailbox.email_password,
                mailbox.pop3_address,
                mailbox.end_uid,
                mailbox.pop3_port.ToString(),
                mailbox.use_ssl?"1":"0",
                mailbox.mailbox_id.ToString());
            return (int)result.InsertID;
        }
        public List<Mailbox> GetAllMailbox()
        {
            var result = Query("select * from user_mailbox");
            var ret = new List<Mailbox>();
            for (int i = 0; i < result.Count; i++)
            {
                ret.Add(Json.Import<string, string>(result[i]).ConvertTo<Mailbox>());
            }
            return ret;
        }
        public List<Mailbox> GetUserMailbox(int user_id)
        {
            var result = QueryStmt("select * from user_mailbox where user_id=@0",user_id.ToString());
            var ret = new List<Mailbox>();
            for (int i = 0; i < result.Count; i++)
            {
                ret.Add(Json.Import<string, string>(result[i]).ConvertTo<Mailbox>());
            }
            return ret;
        }
        public Mailbox GetMailboxByID(int mailbox_id)
        {
            var result = QueryStmt("select * from user_mailbox where mailbox_id=@0", mailbox_id.ToString());
            if (result.Count <= 0)
                return null;
            return Json.Import<string, string>(result[0]).ConvertTo<Mailbox>();
        }
        public bool DeleteMailbox(int mailbox_id)
        {
            QueryStmt("delete from user_mailbox where mailbox_id=@0 limit 1", mailbox_id.ToString());
            return true;
        }
        public bool UpdateMailboxLastUid(int mailbox_id,string uid)
        {
            QueryStmt("update user_mailbox set end_uid=@0 where mailbox_id=@1 limit 1", uid, mailbox_id.ToString());
            return true;
        }
    }
}
