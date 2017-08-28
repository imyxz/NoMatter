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
        public string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("url");
            request.Method = "GET";
            request.Timeout = (int)timeout*1000;
            var response = request.GetResponse();
            return GetResponseString( (HttpWebResponse)response);
        }
        public string Post(string url,string content)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("url");
            request.Method = "POST";
            request.Timeout = (int)timeout * 1000;
            BuildReqStream(ref request, content);
            var response = request.GetResponse();
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
