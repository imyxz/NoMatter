using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using CommonClass;
namespace ServerLib
{
    class MailboxController : ControllerBasic
    {
        public MailboxController() : base()
        {

        }
        public override Dictionary<string, Func<HttpServerContext, string>> GetActions()
        {
            var actions = new Dictionary<string, Func<HttpServerContext, string>>();
            actions["getMailboxs"] = GetMailboxs;
            actions["saveMailbox"] = SaveMailbox;
            actions["deleteMailbox"] = DeleteMailbox;
            return actions;
        }
        public string GetMailboxs(HttpServerContext context)
        {
            try
            {
                var mailbox_model = new MailboxModel(Connection);
                var user_id = (int?)context.session.Data["user_id"] ?? 0;

                if (user_id <= 0) throw new InvalidRequest(1, "还未登录");
                var mailboxs = mailbox_model.GetUserMailbox(user_id);

                var tmp = new Json();
                tmp["mailboxs"] = Json.Import<Mailbox>(mailboxs);
                context.Write(GenerateResponse(tmp).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string SaveMailbox(HttpServerContext context)
        {
            try
            {
                if (context.session.Data["user_id"] <= 0)
                    throw new InvalidRequest(1, "您还未登录");
                var mailbox_model = new MailboxModel(context.DBConnection);
                var mailbox = Json.Decode(context.Request).ConvertTo<Mailbox>();
                var db_mailbox = mailbox_model.GetMailboxByID(mailbox.mailbox_id);
                if (db_mailbox == null)
                {
                    mailbox.user_id = (int)context.session.Data["user_id"];
                    mailbox_model.NewMailbox(mailbox);
                }
                else
                {
                    if (db_mailbox.user_id != (int)context.session.Data["user_id"]) throw new InvalidRequest(1, "您没有权限");
                    mailbox.user_id = db_mailbox.user_id;
                    mailbox_model.SaveMailbox(mailbox);
                }

                context.Write(GenerateResponse(new Json()).Encode());
            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string DeleteMailbox(HttpServerContext context)
        {
            try
            {
                if (context.session.Data["user_id"] <= 0)
                    throw new InvalidRequest(1, "您还未登录");
                var mailbox_model = new MailboxModel(context.DBConnection);
                var mailbox_id =(int) Json.Decode(context.Request)["mailbox_id"];
                var db_mailbox = mailbox_model.GetMailboxByID(mailbox_id);
                if (db_mailbox == null)
                {
                    throw new InvalidRequest(1, "您没有权限");
                }
                else
                {
                    if (db_mailbox.user_id != (int)context.session.Data["user_id"]) throw new InvalidRequest(1, "您没有权限");
                    mailbox_model.DeleteMailbox(mailbox_id);
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
