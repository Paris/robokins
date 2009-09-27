using System;
using System.Text;

namespace robokins.Utility
{
    class AgeSexLocation
    {
        static readonly string[] locations = new string[] {
            "essex",
            "cali",
            "japan",
            "calais",
            "sweden",
            "netherlands",
        };

        static readonly string[] start = new string[] {
            "umm",
            "lol",
            "hey cutie",
            "oh hi",
            "hehe",
        };

        static readonly string[] end = new string[] {
            string.Empty,
            "^_^",
            "<3",
            ":P",
            ":)",
            ";)",
            "xoxox",
        };

        public static string Response()
        {
            StringBuilder resp = new StringBuilder();

            resp.Append(start[Texts.Random.Next(0, start.Length - 1)]);
            resp.Append(' ');
            resp.Append(Texts.Random.Next(8, 14).ToString());
            resp.Append(" f ");
            resp.Append(locations[Texts.Random.Next(0, locations.Length - 1)]);
            resp.Append(' ');
            resp.Append(end[Texts.Random.Next(0, end.Length - 1)]);

            return resp.ToString();
        }
    }
}
