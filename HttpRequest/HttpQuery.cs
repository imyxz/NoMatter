using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace HttpRequest
{
    public class HttpQuery
    {
        private double timeout = 5;
        private CookieCollection requestCookies = new CookieCollection();
        private CookieCollection responseCookies = new CookieCollection();

        public CookieCollection RequestCookies { get => requestCookies; set => requestCookies = value; }
        public CookieCollection ResponseCookies { get => responseCookies; set => responseCookies = value; }

        public string Get(string url)
        {
            var uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(RequestCookies);
            request.Method = "GET";
            request.Timeout = (int)timeout*1000;
            var response = request.GetResponse();
            ResponseCookies = request.CookieContainer.GetCookies(uri);
            return GetResponseString( (HttpWebResponse)response);
        }
        public string Post(string url,string content)
        {
            var uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(RequestCookies);
            request.Method = "POST";
            request.Timeout = (int)timeout * 1000;
            BuildReqStream(ref request, content);
            var response = request.GetResponse();
            ResponseCookies = request.CookieContainer.GetCookies(uri);
            return GetResponseString((HttpWebResponse)response);
        }
        private void BuildReqStream(ref HttpWebRequest webrequest, string request)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(request);
            webrequest.ContentLength = bytes.Length;
            Stream stream = webrequest.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }
        private string GetResponseString( HttpWebResponse response)
        {
            byte[] bytes = new byte[response.ContentLength];
            response.GetResponseStream().Read(bytes, 0, (int)response.ContentLength);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
