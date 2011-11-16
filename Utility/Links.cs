using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace robokins.Utility
{
    class Links
    {
        static Dictionary<string, string> shortened, expanded;

        public static string BitlyAuth { get; set; }
        public static string BitlyKey { get; set; }

        static Links()
        {
            expanded = new Dictionary<string, string>();
            shortened = new Dictionary<string, string>();
            shortened.Add("http://www.autohotkey.net/", "http://ahk.me/sqTsfk");
            shortened.Add("http://www.autohotkey.com/", "http://ahk.me/sDikbQ");
            shortened.Add("http://www.autohotkey.com/forum/", "http://ahk.me/rJiLHk");
            shortened.Add("http://www.autohotkey.com/docs/Tutorial.htm", "http://ahk.me/uKJ4oh");
            shortened.Add("http://github.com/polyethene/robokins", "http://git.io/robo");
        }

        public static string Shorten(string url)
        {
            if (shortened.ContainsKey(url))
                return shortened[url];

            string original = url;
            Uri uri = new Uri(url);

            if (uri.Host.EndsWith("github.com"))
                url = gitio(url);
            else
                url = ahkme(url);

            if (!string.IsNullOrEmpty(url) && !original.Equals(url))
                shortened.Add(original, url);

            return url ?? original;
        }

        public static string Expand(string url)
        {
            if (expanded.ContainsKey(url))
                return expanded[url];

            foreach (string key in shortened.Keys)
            {
                if (shortened[key].Equals(url))
                    return key;
            }

            // TODO: use longurl.org API to expand URL

            return url;
        }

        public static string ahkme(string url)
        {
            if (string.IsNullOrEmpty(BitlyAuth) || string.IsNullOrEmpty(BitlyKey))
            {
                Dictionary<string, string> ahkme = new Dictionary<string, string>(5);
                ahkme.Add("c", @"^https?://(?:www\.)?autohotkey\.(?:net|com)/docs/commands/(\w+)\.htm$");
                ahkme.Add("l", @"^https?://(?:www\.)?autohotkey\.(?:net|com)/(?:~|%7e)Lexikos/AutoHotkey_L/(.*)$");
                ahkme.Add("l", @"^https?://l\.autohotkey\.(?:net|com)/(.*)$");
                ahkme.Add("p", @"^https?://(?:(?:www\.)?autohotkey\.net/paste|paste\.autohotkey\.net)/(.*)$");
                ahkme.Add("u", @"^https?://(?:www\.)?autohotkey\.(?:net|com)/(?:~|%7e)(.*)$");

                foreach (string key in ahkme.Keys)
                {
                    Regex r = new Regex(ahkme[key], RegexOptions.IgnoreCase);
                    Match m = r.Match(url);
                    if (m.Success)
                        return string.Format("http://{0}.ahk.me/{1}", key, m.Groups[1]);
                }
            }

            string bitly = string.Format("http://api.bitly.com/v3/shorten?login={0}&apiKey={1}&longUrl={2}&format=txt", BitlyAuth, BitlyKey, HttpUtility.UrlEncode(url));
            string result = HTTP.DownloadPage(bitly).Trim();

            return result.Length != 0 && result.StartsWith("http://") ? result : null;
        }

        public static string gitio(string url)
        {
            const string service = "http://git.io/";

            var parameters = new Dictionary<string, string>(1);
            parameters.Add("url", url);
            var req = HTTP.RequestPage(service, parameters, "POST");

            return req.StatusCode == HttpStatusCode.Created ? req.Headers[HttpResponseHeader.Location] : null;
        }
    }
}
