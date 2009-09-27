using System;
using robokins.Utility;
using robokins.Utility.Search;

namespace robokins
{
    partial class Program
    {
#if DEBUG
        static void Tests()
        {
            //Console.WriteLine("FML: {0}", FMyLife.Random("") ?? "null");

            //var wiki = Wiki.Search("test");
            //Console.WriteLine("Wiki: {0}", wiki == null ? "null" : string.Join(" ", wiki));

            //var gahk = Google.AutoHotkey("foobar");
            //Console.WriteLine("Google AutoHotkey: {0}", gahk == null ? "null" : string.Join(" ", gahk));

            //Console.WriteLine("Google Define: {0}", Google.Define("test") ?? "null");

            //var msdn = MSDN.Search("foobar");
            //Console.WriteLine("MSDN: {0}", msdn == null ? "null" : string.Join(" ", msdn));

            //var rss = AutoHotkey.LatestRSS();
            //Console.WriteLine("RSS: {0}", string.Join(" - ", rss));

            Console.Read();
        }
#endif
    }
}