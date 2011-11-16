using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace robokins.Utility.Search
{
    class AutoHotkey
    {
        public static string[] GetUserStats(string user)
        {
            const string forum = "http://www.autohotkey.com/forum/";

            var data = new Dictionary<string, string>(10);
            data.Add("search_keywords", string.Empty);
            data.Add("search_terms", "all");
            data.Add("search_author", user);
            data.Add("search_forum", "-1");
            data.Add("search_time", "0");
            data.Add("search_fields", "all");
            data.Add("show_results", "posts");
            data.Add("return_chars", "0");
            data.Add("sort_by", "0");
            data.Add("sort_dir", "DESC");

            var html = HTTP.DownloadPage(string.Concat(forum, "search.php"), data);
            var search = new Regex("<span class=\"maintitle\">Search found (\\d+) match(?:es)?</span>");
            var post = new Regex("</b>&nbsp; &nbsp;Posted: (.+?)&nbsp; &nbsp;Subject: <b><a href=\"post-(\\d+).html[^\"]*?\">([^<]*?)</a></b></span></td>");

            GroupCollection hits = search.Match(html).Groups, stats = post.Match(html).Groups;

            if (hits.Count != 2 || stats.Count != 4)
                return null;

            return new string[]
            {
                hits[1].Value,
                string.Concat(forum, "post-", stats[2].Value, ".html#", stats[2].Value),
                HttpUtility.HtmlDecode(stats[3].Value),
                stats[1].Value
            };
        }
    }
}
