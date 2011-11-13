﻿using System.Web;

namespace robokins.Utility.Search
{
    class Google
    {
        const string ApiKey = "ABQIAAAAFncY3VKcdqJf9_MWTh73ZhRi499a0pNFos5UHqdeDCLX62zzjBT3_7hrzy9T6ZFay81lDGErZKfDKg";
        const string CSE = "008894931886257774458:qsymwz_o1tq";
        const string Site = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&key=" + ApiKey + "&q=";

        public static string[] AutoHotkey(string query)
        {
            return FirstResult(HttpUtility.UrlEncode(query) + "&cx=" + CSE);
        }

        public static string[] Search(string query)
        {
            return FirstResult(HttpUtility.UrlEncode(query));
        }

        static string[] FirstResult(string query)
        {
            string html = HTTP.DownloadPage(Site + query);

            if (html.IndexOf("\"results\":[]") != -1)
                return null;

            const string end = "\",\"";
            string url = Texts.StringBetween(html, "\"url\":\"", end, 0);
            string descr = Texts.StringBetween(html, "\"titleNoFormatting\":\"", end, 0);

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(descr))
                return null;
            else
                return new string[] { url, HttpUtility.HtmlDecode(descr.Replace("\\u0026", "&")) };
        }
    }
}
