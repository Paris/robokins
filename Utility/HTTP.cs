using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace robokins.Utility
{
    class HTTP
    {
        const string UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
        static Dictionary<string, string> urls;

        public static string BitlyAuth { get; set; }
        public static string BitlyKey { get; set; }

        static HTTP()
        {
            urls = new Dictionary<string, string>();
            urls.Add("http://www.autohotkey.net/", "http://ahk.me/sqTsfk");
            urls.Add("http://www.autohotkey.com/", "http://ahk.me/sDikbQ");
            urls.Add("http://www.autohotkey.com/forum/", "http://ahk.me/rJiLHk");
            urls.Add("http://www.autohotkey.com/docs/Tutorial.htm", "http://ahk.me/uKJ4oh");
            urls.Add("http://github.com/polyethene/robokins", "http://ahk.me/s3K0Ze");
        }

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

        public static string ShortUrl(string url)
        {
            if (urls.ContainsKey(url))
                return urls[url];

            #region ahk.me

            Dictionary<string, string> ahkme = new Dictionary<string,string>(3);
            ahkme.Add("c", @"^https?://(?:www\.)?autohotkey\.(?:net|com)/docs/commands/(\w+)\.htm$");
            ahkme.Add("p", @"^https?://(?:(?:www\.)?autohotkey\.net/paste|paste\.autohotkey\.net)/(.*)$");
            ahkme.Add("u", @"^https?://(?:www\.)?autohotkey\.(?:net|com)/(?:~|%7e)(.*)$");

            foreach (string key in ahkme.Keys)
            {
                Regex r = new Regex(ahkme[key], RegexOptions.IgnoreCase);
                Match m = r.Match(url);
                if (m.Success)
                    return string.Format("http://{0}.ahk.me/{1}", key, m.Groups[1]);
            }

            #endregion

            #region bit.ly

            if (string.IsNullOrEmpty(BitlyAuth) || string.IsNullOrEmpty(BitlyKey))
                return url;

            string bitly = string.Format("http://api.bitly.com/v3/shorten?login={0}&apiKey={1}&longUrl={2}&format=txt", BitlyAuth, BitlyKey, HttpUtility.UrlEncode(url));
            string result = DownloadPage(bitly).Trim();

            if (result.Length != 0 && result.StartsWith("http://"))
            {
                urls.Add(url, result);
                return result;
            }

            #endregion

            return url;
        }
    }
}
