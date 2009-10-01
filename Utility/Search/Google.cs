using System.Web;

namespace robokins.Utility.Search
{
    class Google
    {
        const string ApiKey = "ABQIAAAAFncY3VKcdqJf9_MWTh73ZhRi499a0pNFos5UHqdeDCLX62zzjBT3_7hrzy9T6ZFay81lDGErZKfDKg";
        const string CSE = "008894931886257774458:qsymwz_o1tq";

        public static string[] AutoHotkey(string query)
        {
            string html = HTTP.DownloadPage("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&key=" +
                ApiKey + "&cx=" + CSE + "&q=" + HttpUtility.UrlEncode(query));

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

        public static string Define(string term)
        {
            string html = HTTP.DownloadPage("http://www.google.com/search?q=define%3A" + HttpUtility.UrlEncode(term));

            const string bound = "<li>";
            string result = Texts.StringBetween(html, bound, bound, 0);

            if (string.IsNullOrEmpty(result))
                return null;

            int link = result.IndexOf(" ...<br>");
            if (link > 0)
                result = result.Substring(0, link);

            return result.Length == 0 ? null : HttpUtility.HtmlDecode(result);
        }
    }
}
