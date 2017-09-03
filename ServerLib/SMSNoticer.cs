using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace ServerLib
{
    static public class SMSNoticer
    {
        static public bool SendSMS(string receiver, string Subject, string Body)
        {

            ITopClient client = new DefaultTopClient("	http://gw.api.taobao.com/router/rest", "23332091", "398e14719c701a0c8f55ef99ca51f25f");
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "123456";
            req.SmsType = "normal";
            req.SmsFreeSignName = "水晶监视";
            var time = DateTime.Now.ToString("hh:mm");
            req.SmsParam = "{\"name\":\" " + Subject.Replace("\"","\\\"") +" \",\"time\":\" "+ time +" \"}";
            req.RecNum = receiver;
            req.SmsTemplateCode = "SMS_91795055";
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            return true;
        }
        
    }
}

