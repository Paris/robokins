using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

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

        public static HttpWebResponse RequestPage(string uri, IDictionary<string, string> parameters, string method = "POST")
        {
            var data = new StringBuilder();

            foreach (var key in parameters.Keys)
            {
                data.Append(key);
                data.Append('=');
                data.Append(HttpUtility.UrlEncode(parameters[key]));
                data.Append('&');
            }

            data.Remove(data.Length - 1, 1);

            ServicePointManager.Expect100Continue = false;
            byte[] buffer = Encoding.ASCII.GetBytes(data.ToString());
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);

            req.Method = method;

            if (req.Method.Equals("POST"))
                req.ContentType = "application/x-www-form-urlencoded";

            req.ContentLength = buffer.Length;

            Stream post = req.GetRequestStream();
            post.Write(buffer, 0, buffer.Length);
            post.Close();

            return (HttpWebResponse)req.GetResponse();
        }

        public static string DownloadPage(string uri, IDictionary<string, string> parameters, string method = "POST")
        {
            try
            {
                var resp = RequestPage(uri, parameters, method);
                return (new StreamReader(resp.GetResponseStream())).ReadToEnd();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
