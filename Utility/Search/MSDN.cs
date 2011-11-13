using System.Text.RegularExpressions;
using System.Web;

namespace robokins.Utility.Search
{
    class MSDN
    {
        const string URL = "http://social.msdn.microsoft.com/search/en-US/feed?refinement=181&format=RSS&query=";

        public static string[] Search(string query)
        {
            string xml = HTTP.DownloadPage(URL + HttpUtility.UrlEncode(query));

            GroupCollection group = Texts.ItemDescrRSS.Match(xml).Groups;
            if (group.Count < 4 || group[2].Value == string.Empty)
                return null;

            return new string[] { group[2].Value, HttpUtility.HtmlDecode(group[1].Value), HttpUtility.HtmlDecode(group[3].Value) };
        }
    }
}
