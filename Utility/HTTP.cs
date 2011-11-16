using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace robokins.Utility
{
    class HTTP
    {
        public static string UserAgentReal;
        public const string UserAgentFake = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";

        public HTTP()
        {
            Assembly self = Assembly.GetExecutingAssembly();
            UserAgentReal = string.Concat(((AssemblyTitleAttribute)Attribute.GetCustomAttribute(self, typeof(AssemblyTitleAttribute))).Title, "/", self.GetName().Version.ToString());
        }

        public static string DownloadPage(string uri)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);
                req.UserAgent = UserAgentFake;

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                    return string.Empty;

                string res = (new StreamReader(resp.GetResponseStream())).ReadToEnd();
                resp.Close();
                return res;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string DownloadPage(string uri, string data, NetworkCredential auth = null, string contentType = null)
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);

                if (auth != null)
                    req.Credentials = auth;

                req.Method = "POST";
                req.ContentType = contentType ?? "application/x-www-form-urlencoded";
                req.ContentLength = buffer.Length;

                Stream post = req.GetRequestStream();
                post.Write(buffer, 0, buffer.Length);
                post.Close();

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return (new StreamReader(resp.GetResponseStream())).ReadToEnd();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
