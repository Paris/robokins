using System.Web;

namespace robokins.Utility.Search
{
    class UrbanDictionary
    {
        public static string Search(string term)
        {
            var html = HTTP.DownloadPage("http://www.urbandictionary.com/define.php?term=" + HttpUtility.UrlEncode(term));
            var z = html.IndexOf("<table id='entries'>");
            if (z == -1)
                return null;
            html = Texts.StringBetween(html, "<div class=\"definition\">", "</div>", z);
            if (string.IsNullOrEmpty(html))
                return null;
            html = Texts.StripTags.Replace(html, string.Empty);
            return HttpUtility.HtmlDecode(html);
        }
    }
}