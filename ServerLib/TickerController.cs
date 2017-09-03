using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using System.Diagnostics;
using System.Security.Cryptography;
using CommonClass;
namespace ServerLib
{
    class TickerController : ControllerBasic
    {
        public TickerController() : base()
        {

        }
        public override Dictionary<string, Func<HttpServerContext, string>> GetActions()
        {
            var actions = new Dictionary<string, Func<HttpServerContext, string>>();
            actions["addMessage"] = AddMessage;
            actions["sendSms"] = SendSMS;
            actions["sendEmail"] = SendEmail;
            actions["receiveMail"] = ReceiveMail;
            return actions;
        }

        public string AddMessage(HttpServerContext context)
        {
            try
            {
                var matter_model = new MatterModel(context.DBConnection);
                var message_model = new MessageModel(context.DBConnection);
                var ret = matter_model.GetNeedNoticeMatters();
                matter_model.SetNeedNoticeMattersNoticed();
                foreach(var matter in ret)
                {
                    var message = new Message();
                    message.create_time = DateTime.Now;
                    message.user_id = matter.user_id;
                    message.message_title = matter.matter_name;
                    message.message_body = "您的事件 " + matter.matter_name + " 于 " + matter.matter_next_effect_time.ToString("hh:MM") + " 到期";
                    message.is_sended_client = message.is_sended_email = message.is_sended_sms = false;
                    message_model.NewMessage(message);
                }
                context.Write(GenerateResponse(new Json()).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string SendEmail(HttpServerContext context)
        {
            try
            {
                var message_model = new MessageModel(context.DBConnection);
                var user_model = new UserModel(context.DBConnection);
                var ret = message_model.GetEmailMessage();
                foreach (var message in ret)
                {
                    var user_info = user_model.GetUserInfoByUserID(message.user_id);
                    if(user_info!=null && user_info.user_email!="")
                    {
                        MailNoticer.SendMail(user_info.user_email, message.message_title, message.message_body);
                    }
                    message_model.SetMessageSendedEmail(message.message_id);
                }
                context.Write(GenerateResponse(new Json()).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string SendSMS(HttpServerContext context)
        {
            try
            {
                var message_model = new MessageModel(context.DBConnection);
                var user_model = new UserModel(context.DBConnection);
                var ret = message_model.GetSMSMessage();
                foreach (var message in ret)
                {
                    var user_info = user_model.GetUserInfoByUserID(message.user_id);
                    if (user_info != null && user_info.user_phone != "")
                    {
                        SMSNoticer.SendSMS(user_info.user_phone, message.message_title, message.message_body);
                    }
                    message_model.SetMessageSendedSms(message.message_id);
                }
                context.Write(GenerateResponse(new Json()).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string ReceiveMail(HttpServerContext context)
        {
            try
            {
                var mailbox_model = new MailboxModel(context.DBConnection);
                var matter_model = new MatterModel(context.DBConnection);
                var ret = mailbox_model.GetAllMailbox();
                foreach (var mailbox in ret)
                {
                    try
                    {
                        var mails=EmailReceiver.GetEmails(mailbox.Email_address, mailbox.email_password, mailbox.pop3_address, mailbox.pop3_port, mailbox.use_ssl, mailbox.end_uid, 20);
                        if(mails.Count>0)
                        {
                            mailbox_model.UpdateMailboxLastUid(mailbox.mailbox_id, mails[0].email_uid);
                            foreach(var matter in mails)
                            {
                                matter.user_id = mailbox.user_id;
                                matter.is_noticed = 1;
                                matter.matter_addion_info["email_uid"] = matter.email_uid;
                                matter_model.NewMatter(matter);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                context.Write(GenerateResponse(new Json()).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
    }
}
