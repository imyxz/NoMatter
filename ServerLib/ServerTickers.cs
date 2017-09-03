using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpRequest;
namespace ServerLib
{
    static class ServerTickers
    {
        public static string serverAddress = "http://127.0.0.1:9090/";
        public static bool AddMessage()
        {
            var http = new HttpQuery();
            http.timeout = 120;
            http.Post(serverAddress + "tickers/addMessage", "");
            return true;
        }
        public static bool SendEmail()
        {
            var http = new HttpQuery();
            http.timeout = 120;
            http.Post(serverAddress + "tickers/sendEmail", "");
            return true;
        }
        public static bool SendSMS()
        {
            var http = new HttpQuery();
            http.timeout = 120;
            http.Post(serverAddress + "tickers/sendSms", "");
            return true;
        }
        public static bool ReceiveMail()
        {
            var http = new HttpQuery();
            http.timeout = 120;
            http.Post(serverAddress + "tickers/receiveMail", "");
            return true;
        }
    }
}
