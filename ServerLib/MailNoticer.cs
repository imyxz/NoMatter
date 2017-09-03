using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
namespace ServerLib
{
    static public class MailNoticer
    {
        static public bool SendMail(string receiver,string Subject, string Body)
        {
            bool flag = false;
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.qq.com";
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                //client.Port = 465;
                
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("89571747@qq.com", "zhktztvmojfobjdc");
                MailMessage Message = new MailMessage();
                Message.From = new System.Net.Mail.MailAddress("89571747@qq.com");
                Message.To.Add(receiver);
                Message.Subject = Subject;
                Message.Body = Body;
                Message.SubjectEncoding = System.Text.Encoding.UTF8;
                Message.BodyEncoding = System.Text.Encoding.UTF8;
                Message.Priority = System.Net.Mail.MailPriority.High;
                Message.IsBodyHtml = true;
                client.Timeout = 20*1000;
                
                client.Send(Message);
                client.Dispose();
                flag = true;
            }
            catch { }
            return flag;
            
        }
    }
}
