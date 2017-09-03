using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ServerLib
{
    public class MatterServer
    {
        static Dictionary<string,IController> Controllers = new Dictionary<string, IController>();
        private HttpServer http;
        static List<Func<bool>> Tickers = new List<Func<bool>>();

        static MatterServer()
        {
            Controllers["user"]=new UserController();
            Controllers["matter"] = new MatterController();
            Controllers["tickers"] = new TickerController();
            Controllers["mailbox"] = new MailboxController();
            Tickers.Add(ServerTickers.AddMessage);
            Tickers.Add(ServerTickers.SendEmail);
            Tickers.Add(ServerTickers.SendSMS);
            Tickers.Add(ServerTickers.ReceiveMail);
        }
        public void Start(params string [] prefixes)
        {
            http = new HttpServer(prefixes);
            foreach(var pair in Controllers)
            {
                http.Registe(pair.Key, pair.Value);
            }
            foreach (var ticker in Tickers)
            {
                http.AddTicker(ticker);
            }
            http.Start();
        }
    }
}
