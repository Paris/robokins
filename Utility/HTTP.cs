using System;
using System.IO;
using System.Net;
using System.Text;

namespace robokins.Utility
{
    class HTTP
    {
        const string UserAgent = "Mozilla/5.0 (X11; U; Linux x86_64; en-GB; rv:1.9.1.3) Gecko/20090913 Firefox/3.5.3";

        public static string DownloadPage(string uri)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);
                req.UserAgent = UserAgent;
                

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                    return string.Empty;

                string res = (new StreamReader(resp.GetResponseStream())).ReadToEnd();
                resp.Close();
                return res;
            }
            catch (Exception) { return string.Empty; }
        }

        public static string DownloadPage(string uri, string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = buffer.Length;

            Stream post = req.GetRequestStream();
            post.Write(buffer, 0, buffer.Length);
            post.Close();

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            return (new StreamReader(resp.GetResponseStream())).ReadToEnd();
        }
    }
}
