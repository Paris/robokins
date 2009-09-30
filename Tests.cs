using System;
using System.Diagnostics;
using robokins.Utility;
using robokins.Utility.Search;

namespace robokins
{
    partial class Program
    {
        [Conditional("DEBUG")]
        static void Tests()
        {
            string[] defs;
            string def;

            Console.WriteLine("FML: {0}", FMyLife.Random("") ?? "null");
            Console.WriteLine();

            defs = Wiki.Search("test");
            Console.WriteLine("Wiki: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            defs = Google.AutoHotkey("foobar");
            Console.WriteLine("Google AutoHotkey: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            Console.WriteLine("Google Define: {0}", Google.Define("test") ?? "null");
            Console.WriteLine();

            defs = MSDN.Search("createwindow");
            Console.WriteLine("MSDN: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            def = "dllcall()";
            defs = Manual.Lookup(def);
            Console.WriteLine("Manual \"{0}\": {1}: {2}", def, defs[0], defs[1]);
            def = "nothing";
            Console.WriteLine("Manual \"{0}\": {1}", def, Manual.Lookup(def) == null ? "null" : "not null");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Connect to IRC [y/n]?");
            char mode = (char)Console.Read();
            if (!(mode == 'y' || mode == 'Y'))
                Environment.Exit(0);
        }
    }
}
