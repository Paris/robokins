﻿using System;
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

            defs = MSDN.Search("createwindow");
            Console.WriteLine("MSDN: {0}", defs == null ? "null" : string.Join(" ", defs));
            Console.WriteLine();

            def = UrbanDictionary.Search("windows");
            Console.WriteLine("UrbanDictionary: {0}", def);
            Console.WriteLine();

            defs = AutoHotkey.GetUserStats("Chris");
            Console.WriteLine("Chris: {0} {1} {2} {3}", defs);
            Console.WriteLine();

            def = Links.Shorten("http://autohotkey.net/docs/commands/FormatTime.htm");
            Console.WriteLine("Short URL: {0}", def);
            Console.WriteLine();

            def = Links.Shorten("http://www.autohotkey.net/paste/uqnfwf");
            Console.WriteLine("Short URL: {0}", def);
            Console.WriteLine();

            def = Links.Shorten("http://www.autohotkey.net/~test/");
            Console.WriteLine("Short URL: {0}", def);
            Console.WriteLine();

            def = Links.Shorten("https://gist.github.com/187052");
            Console.WriteLine("Short URL: {0}", def);
            Console.WriteLine();

            def = Links.Shorten("http://maps.google.co.uk/maps?f=q&source=s_q&hl=en&geocode=&q=louth&sll=53.800651,-4.064941&sspn=33.219383,38.803711&ie=UTF8&hq=&hnear=Louth,+United+Kingdom&ll=53.370272,-0.004034&spn=0.064883,0.075788&z=14");
            Console.WriteLine("Short URL: {0}", def);
            Console.WriteLine();

            var pastes = Bot.pasteFetch(1);
            foreach (var item in pastes.Keys)
                Console.WriteLine("Paste: {0} by {1}{2}", item, pastes[item][0], string.IsNullOrEmpty(pastes[item][1]) ? string.Empty : " - " + pastes[item][1]);
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
