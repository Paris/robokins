using System.Text;
using System.Web;

namespace robokins.Utility.Search
{
    class FMyLife
    {
        const string viewsite = "http://www.fmylife.com/";
        const string site = "http://api.betacie.com/view/";
        const string key = "readonly";
        const string query = "?key=" + key + "&language=en";

        public static string Random()
        {
            return Get("random/nocomment");
        }

        public static string Random(string type)
        {
            if (type.Length == 0)
                return Random();

            switch (type)
            {
                case "love":
                case "money":
                case "kids":
                case "work":
                case "health":
                case "sex":
                case "intimacy":
                case "miscellaneous":
                    return Get(type + "/1/nocomment");

                default:
                    return Random();
            }
        }

        static string Get(string method)
        {
            var uri = new StringBuilder(site.Length + method.Length + query.Length);
            uri.Append(site);
            uri.Append(method);
            if (method.IndexOf('?') == -1)
                uri.Append(query);
            else
            {
                uri.Append('&');
                uri.Append(query, 1, query.Length - 1);
            }

            string xml = HTTP.DownloadPage(uri.ToString());

            string text = Texts.StringBetween(xml, "<text>", "</text>", 0); // don't ask, I gave up on XmlReader
            if (string.IsNullOrEmpty(text))
                return null;

            string cat = Texts.StringBetween(xml, "<category>", "</category>", 0);
            if (string.IsNullOrEmpty(cat))
                return null;

            string id = Texts.StringBetween(xml, "\"", "\"", xml.IndexOf("<item id="));
            if (string.IsNullOrEmpty(id))
                return null;
            
            return string.Concat(HttpUtility.HtmlDecode(text), " ", viewsite, cat, "/", id);
        }
    }
}
