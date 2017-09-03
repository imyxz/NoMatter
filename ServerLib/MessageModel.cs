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
    class MessageModel : ModelBasic
    {
        public MessageModel(MySqlConnection connect) : base(connect)
        {
        }
        public int NewMessage(Message message)
        {
            var result = QueryStmt("insert into user_message set user_id=@0,message_title=@1,message_body=@2,create_time=@3,is_sended_email=@4,is_sended_sms=@5,is_sended_client=@6",
                message.user_id.ToString(), message.message_title, message.message_body, message.create_time.ToString(), message.is_sended_email.ToString(), message.is_sended_sms.ToString(), message.is_sended_client.ToString());
            return (int)result.InsertID;
        }
        public List<Message> GetEmailMessage()
        {
            var result = Query("select * from user_message where is_sended_email=false");
            var ret = new List<Message>();
            for (int i = 0; i < result.Count; i++)
            {
                ret.Add(Json.Import<string,string>(result[i]).ConvertTo<Message>());
            }
            return ret;
        }
        public List<Message> GetSMSMessage()
        {
            var result = Query("select * from user_message where is_sended_sms=false");
            var ret = new List<Message>();
            for (int i = 0; i < result.Count; i++)
            {
                ret.Add(Json.Import<string, string>(result[i]).ConvertTo<Message>());
            }
            return ret;
        }
        public List<Message> GetUserClientMessage(int user_id)
        {
            var result = QueryStmt("select * from user_message where user_id=@0 and is_sended_sms=false",user_id.ToString());
            var ret = new List<Message>();
            for (int i = 0; i < result.Count; i++)
            {
                ret.Add(Json.Import<string, string>(result[i]).ConvertTo<Message>());
            }
            return ret;
        }
        public void SetMessageSendedEmail(int message_id)
        {
            QueryStmt("update user_message set is_sended_email=true where message_id=@0", message_id.ToString());
        }
        public void SetMessageSendedSms(int message_id)
        {
            QueryStmt("update user_message set is_sended_sms=true where message_id=@0", message_id.ToString());
        }
        public void SetMessageSendedClient(int message_id)
        {
            QueryStmt("update user_message set is_sended_client=true where message_id=@0", message_id.ToString());
        }

    }
}
