using System.Text.RegularExpressions;
using System.Web;

namespace robokins.Utility.Search
{
    class AutoHotkey
    {
        static Regex search = new Regex("<span class=\"maintitle\">Search found (\\d+) matches</span>");
        static Regex post = new Regex("</b>&nbsp; &nbsp;Posted: (.+?)&nbsp; &nbsp;Subject: <b><a href=\"post-(\\d+).html[^\"]*?\">([^<]*?)</a></b></span></td>");
        static Regex forumstats = new Regex("<span class=\"gensmall\">In total there are [^\r\n]+?</span></td>");
        static Regex statsafter = new Regex("\\s+&nbsp;.+?\\](?=Reg)");

        public static string[] UserStats(string user)
        {
            string forum = "http://www.autohotkey.com/forum/",
                data = "search_keywords=&search_terms=all&search_author=" +
                HttpUtility.UrlEncode(user) + "&search_forum=-1&search_time=0&search_fields=all&show_results=posts" +
                "&return_chars=0&sort_by=0&sort_dir=DESC";

            string html = HTTP.DownloadPage(forum + "search.php", data);

            GroupCollection hits = search.Match(html).Groups, stats = post.Match(html).Groups;
            if (hits.Count != 2 || stats.Count != 4)
                return null;

            return new string[] { hits[1].Value, 
                forum + "post-" + stats[2].Value + ".html#" + stats[2].Value,
                HttpUtility.HtmlDecode(stats[3].Value),
                stats[1].Value };
        }

        public static string[] LatestRSS()
        {
            string html = HTTP.DownloadPage("http://www.autohotkey.com/forum/rss.php?t=1");

            GroupCollection group = Texts.ItemRSS.Match(html).Groups;
            string[] res = new string[] { HttpUtility.HtmlDecode(group[1].Value), HttpUtility.HtmlDecode(group[2].Value) };
            res[0] = res[0].Substring(res[0].IndexOf(" :: ") + 4).Replace("&quot;", "\"");

            return res;
        }

        public static string ForumStats()
        {
            string html = HTTP.DownloadPage("http://www.autohotkey.com/forum/");

            GroupCollection group = forumstats.Match(html).Groups;
            if (group.Count == 0)
                return null;

            string stats = Texts.StripTags.Replace(group[0].Value, string.Empty);
            stats = statsafter.Replace(stats, "; ");

            const int maxlength = 500;
            if (stats.Length > maxlength)
                stats = string.Concat(stats.Substring(0, maxlength - 4), " ...");

            return stats;
        }
    }
}
