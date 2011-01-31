using System;
using System.Diagnostics;
using System.Security;
using robokins.Utility;
using robokins.Utility.Search;

namespace robokins
{
    partial class Program
    {
        [Conditional("DEBUG")]
        static void Tests(SecureString password)
        {
            string[] defs;
            string def;

            defs = Wiki.Search("test");
            Console.WriteLine("Wiki: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            defs = Google.AutoHotkey("foobar");
            Console.WriteLine("Google AutoHotkey: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            defs = Google.Search("meaning of life");
            Console.WriteLine("Google Search: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            Console.WriteLine("Google Define: {0}", Google.Define("test") ?? "null");
            Console.WriteLine();

            defs = MSDN.Search("createwindow");
            Console.WriteLine("MSDN: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            def = UrbanDictionary.Search("windows");
            Console.WriteLine("UrbanDictionary: {0}", def);
            Console.WriteLine();

            def = "dllcall()";
            defs = Manual.Lookup(def);
            Console.WriteLine("Manual \"{0}\": {1}: {2}", def, defs[0], defs[1]);
            def = "nothing";
            Console.WriteLine("Manual \"{0}\": {1}", def, Manual.Lookup(def) == null ? "null" : "not null");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Connect to IRC [y/n]? ");

            char mode = (char)Console.Read();
            if (!(mode == 'y' || mode == 'Y'))
                Environment.Exit(0);
        }
    }
}
