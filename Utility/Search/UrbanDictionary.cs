using System.Web;

namespace robokins.Utility.Search
{
    class UrbanDictionary
    {
        public static string Search(string term)
        {
            string html = HTTP.DownloadPage("http://www.urbandictionary.com/define.php?term=" + HttpUtility.UrlEncode(term));

            const string boundary = "<div class='definition'>";
            int pos = html.IndexOf(boundary);
            if (pos == -1)
                return null;

            pos += boundary.Length;
            html = html.Substring(pos, html.IndexOf("</div>", pos) - pos).Replace("<br>", "\n").Trim();
            html = Texts.StripTags.Replace(html, string.Empty);
            pos = html.IndexOf("\n");
            if (pos != -1)
                html = html.Substring(0, pos);

            return HttpUtility.HtmlDecode(html);
        }
    }
}
