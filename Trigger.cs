using System;
using System.Reflection;
using System.Text;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        void Trigger(ReceivedMessage message)
        {
            #region Variables

            if (!Invoke(message))
                return;

            bool notify = false;
            bool action = false;
            bool auth = Operators.IndexOf(string.Concat(Delimiter, message.User.Host, Delimiter)) != -1;
            bool search = false;

            string[] command = Utility.Texts.Commands(message.Text);
            string response = string.Empty;
            string def;
            string[] defs;

            #endregion

            switch (command[0].Trim().ToLowerInvariant())
            {
                #region Operator functions

                case "quit":
                case "die":
                    if (auth)
                        quit = true;
                    else
                    {
                        response = "You do not have the authority to make me quit.";
                        notify = true;
                    }
                    break;

                case "quiet":
                case "mute":
                case "m":
                    if (auth)
                    {
                        if (command[1].Length == 0)
                        {
                            response = "Please specify a mask to mute. Enter /msg ChanServ HELP QUIET for more information.";
                            notify = true;
                        }
                        else
                            Message("ChanServ", "QUIET " + Channel + " " + command[1]);
                    }
                    else
                    {
                        response = "You do not have the authority to mute users on the channel.";
                        notify = true;
                    }
                    break;

                case "unquiet":
                case "unmute":
                case "um":
                    if (auth)
                    {
                        if (command[1].Length == 0)
                        {
                            response = "Please specify a mask to unmute. Enter /msg ChanServ HELP UNQUIET for more information.";
                            notify = true;
                        }
                        else
                            Message("ChanServ", "UNQUIET " + Channel + " " + command[1]);
                    }
                    else
                    {
                        response = "You do not have the authority to unmute users on the channel.";
                        notify = true;
                    }
                    break;

                case "say":
                    if (auth)
                    {
                        if (command[2].Length == 0)
                        {
                            response = "You have not told me what to repeat.";
                            notify = true;
                        }
                        else
                            response = command[2];
                    }
                    else
                    {
                        response = "Sorry, I cannot repeat what you said.";
                        notify = true;
                    }
                    break;

                #endregion

                #region Messages

                case "status":
                case "stats":
                case "stat":
                case "s":
                    response = string.Format("Uptime: " + Utility.Font.Bold + "{0}" + Utility.Font.Bold, 
                        Utility.Time.ToDays(Math.Abs(Utility.Time.TimeSpanNow().Subtract(start).TotalSeconds)));
                    break;

                case "hello":
                case "who":
                case "hey":
                case "sup":
                case "hi":
                    response = string.Format("Hi, I'm a helper bot for #{0}.", ChannelName);
                    break;

                case "pastebin":
                case "paste":
                case "pb":
                case "p":
                    response = "Please use the official AutoHotkey pastebin at " + PasteURI + " to share code.";
                    break;

                case "rules":
                case "rule":
                    response = "This is a PG rated channel, please do not swear or post links to material unsuitable for a younger audience. " +
                        "Security related topics can only be discussed for educational purposes i.e. no black hat.";
                    break;

                case "help":
                    response = "Hello, how can we help? If you have not already please read the tutorial at http://www.autohotkey.com/docs/Tutorial.htm";
                    break;

                case "about":
                    Assembly self = Assembly.GetExecutingAssembly();
                    StringBuilder about = new StringBuilder();
                    about.Append(((AssemblyTitleAttribute)Attribute.GetCustomAttribute(self, typeof(AssemblyTitleAttribute))).Title);
                    about.Append(" v");
                    about.Append(self.GetName().Version.ToString());
                    about.Append(" - ");
                    about.Append(((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(self, typeof(AssemblyDescriptionAttribute))).Description);
                    about.Append(" by ");
                    about.Append(((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(self, typeof(AssemblyCompanyAttribute))).Company);
                    about.Append(". See ");
                    about.Append(((AssemblyProductAttribute)Attribute.GetCustomAttribute(self, typeof(AssemblyProductAttribute))).Product);
                    response = about.ToString();
                    break;

                #endregion

                #region Fun

                case "smile":
                case "happy":
                case "dance":
                case "kirby":
                    response = string.Format("{0}2(>^_^)> {0}3<(^_^<) {0}4^(^_^)^ {0}5v(^_^)v {0}6<(^_^<) {0}7(>^_^)> {0}8^(^_^)> {0}9<(^_^)^ {3}{1}{2}{1} :D",
                        Utility.Font.Colour, Utility.Font.Bold, auth && command[2].Length != 0 ? command[2] : "SUPER HAPPY FUN!", Utility.Font.Normal);
                    break;

                case "flip":
                case "coin":
                    response = string.Concat("flips a coin: " + Utility.Font.Colour, Utility.Texts.Random.Next() % 2 == 0 ? "3HEADS" : "4TAILS");
                    action = true;
                    break;

                case "fmylife":
                case "fml":
                    def = Utility.Search.FMyLife.Random(command[1]);
                    if (def.Length == 0)
                    {
                        response = "Unable to retrieve FMyLife at this time.";
                        notify = true;
                    }
                    else
                        response = def;
                    break;

                case "magicball":
                case "eightball":
                case "8ball":
                case "ball":
                case "8":
                    response = Utility.EightBall.Reponse(command[2]);
                    break;

                case "troutslap":
                case "trout":
                case "slaps":
                case "slap":
                    response = Utility.Slap.Response(command[1].Length == 0 ? message.User.Nick : command[1]);
                    action = true;
                    break;

                #endregion

                #region Utilities

                case "random":
                case "rand":
                    response = string.Format("Random integer: {0}{1}{0} double: {0}{2}{0}",
                        Utility.Font.Bold, Utility.Texts.Random.Next(), Utility.Texts.Random.NextDouble());
                    break;

                case "clock":
                case "time":
                case "tiem":
                case "t":
                    response = Utility.Time.WorldTime();
                    break;

                #endregion

                #region Stubs

                case "c":
                case "calc":
                case "what":
                case "?":
                case "=":
                    response = "Sorry, I am currently unable to calculate equations.";
                    notify = true;
                    break;

                case "xdcc":
                    response = "Sorry, I cannot send any files.";
                    notify = true;
                    break;

                case "tell":
                    response = "Sorry, tell is currently disabled.";
                    notify = true;
                    break;

                case "rr":
                case "loli":
                case "onee":
                case "lion":
                case "imouto":
                case "deer":
                case "shota":
                case "pantsu":
                case "stuff":
                case "stuffstats":
                case "top10":
                case "quote":
                case "qtop10":
                case "qlatest":
                case "mio":
                    return;

                #endregion

                #region Search

                case "define":
                case "def":
                case "d":
                    if (command[2].Length == 0)
                    {
                        response = "Please specify a search term.";
                        notify = true;
                        break;
                    }
                    def = Utility.Search.Google.Define(command[2]);
                    if (string.IsNullOrEmpty(def))
                    {
                        response = "Could not find a definition for " + Utility.Font.Bold + command[2] + Utility.Font.Bold;
                        notify = true;
                    }
                    else
                        response = string.Format("{0}{1}{0}: {2}", new string[] { Utility.Font.Underlined, command[2], def });
                    break;

                case "google":
                case "g":
                    if (command[2].Length == 0)
                    {
                        response = "Please specify a search term.";
                        notify = true;
                        break;
                    }
                    defs = Utility.Search.Google.Search(command[2]);
                    if (defs == null)
                    {
                        response = "Could not find a definition for " + Utility.Font.Bold + command[2] + Utility.Font.Bold;
                        notify = true;
                    }
                    else
                        response = string.Format("{0} - {1}", defs[1], defs[0]);
                    break;

                case "user":
                case "u":
                    if (command[2].Length == 0)
                    {
                        response = "Please specify a search term.";
                        notify = true;
                        break;
                    }
                    defs = Utility.Search.AutoHotkey.UserStats(command[2]);
                    if (defs == null)
                    {
                        response = "Could not find user " + Utility.Font.Bold + command[2] + Utility.Font.Bold;
                        notify = true;
                    }
                    else
                    {
                        response = string.Format("{0}{7}{0} made {1}6{2}{3}{2}{1} post{8}; {2}{4}{2} - {5} {1}14 on {6}",
                            new string[] { Utility.Font.Underlined, Utility.Font.Colour, Utility.Font.Bold,
                                defs[0], defs[2], defs[1], defs[3], command[2], defs[0] == "1" ? string.Empty : "s" });
                    }
                    break;

                case "wikipedia":
                case "wiki":
                case "w":
                    if (command[2].Length == 0)
                    {
                        response = "Please specify a search term.";
                        notify = true;
                        break;
                    }
                    defs = Utility.Search.Wiki.Search(command[2]);
                    if (defs == null)
                    {
                        response = "Could not find a definition on the Wikipedia for " + Utility.Font.Bold + command[2] + Utility.Font.Bold;
                        notify = true;
                    }
                    else
                        response = string.Format("{0}{1}{0}: {2} - {3}",
                            new string[] { Utility.Font.Underlined, command[2], defs[0], defs[1] });
                    break;

                case "urbandictionary":
                case "urban":
                case "ub":
                    if (command[2].Length == 0)
                    {
                        response = "Please specify a search term.";
                        notify = true;
                        break;
                    }
                    def = Utility.Search.UrbanDictionary.Search(command[2]);
                    if (def == null)
                    {
                        response = "Could not find a definition on the Urban Dictionary for " + Utility.Font.Bold + command[2] + Utility.Font.Bold;
                        notify = true;
                    }
                    else
                    {
                        def = def.Replace('\r', ' ');
                        def = def.Replace('\n', ' ');
                        if (def.Length > 450)
                            def = string.Concat(def.Substring(0, 450 - 4), " ...");
                        response = string.Format("{0}{1}{0}: {2}", Utility.Font.Underlined, command[2], def);
                    }
                    break;

                case "winapi":
                case "msdn":
                    if (command[2].Length == 0)
                    {
                        response = "Please specify a search term.";
                        response = Utility.Font.Colour + "5An argument is required for this command.";
                        notify = true;
                        break;
                    }
                    defs = Utility.Search.MSDN.Search(command[2]);
                    if (defs == null)
                    {
                        response = "Could not find a definition on the MSDN for " + Utility.Font.Bold + command[2] + Utility.Font.Bold;
                        notify = true;
                    }
                    else
                        response = string.Format("{3}{0}{3}: {2} - {1}",
                            new string[] { defs[0], defs[1], defs[2], Utility.Font.Bold });
                    break;

                case "search":
                case "find":
                case "query":
                case "look":
                    search = true;
                    goto default;

                default:
                    def = (search ? command[2] : message.Text).Trim().ToLowerInvariant();
                    if (def.Length < 2)
                        break;

                    const int min = 2;
                    int letters = 0;
                    bool valid = false;
                    for (int i = 0; i < def.Length; i++)
                        if (char.IsLetter(def, i))
                        {
                            if (++letters >= min)
                            {
                                valid = true;
                                break;
                            }
                        }
                    if (!valid)
                        break;

                    defs = Utility.Manual.Lookup(def);
                    if (defs != null && defs.Length == 2)
                        response = string.Format("\x02{0}6{1}{0}\x02: " + Website + "{2}", Utility.Font.Colour, defs[0], defs[1]);
                    else
                    {
                        defs = Utility.Search.Google.AutoHotkey(def);
                        if (defs == null)
                        {
                            response = "Could not find " + Utility.Font.Bold + def + Utility.Font.Bold;
                            notify = true;
                        }
                        else
                            response = string.Format("Found \"{0}{1}{0}\": {2}", Utility.Font.Bold, defs[1], defs[0]);
                    }
                    break;

                #endregion
            }

            #region Message

            if (message.Target == Nick)
                notify = true;

            if (notify)
                message.Target = message.User.Nick;

            if (message.Target[0] != '#')
                notify = true;

            if (action && !notify) // since /me doesn't work in notice
                response = Action(response);

            if (response.Length != 0)
                Message(message.Target, response, notify);

            #endregion
        }
    }
}
