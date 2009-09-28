using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        void Trigger(ReceivedMessage message)
        {
            #region Variables

            if (!Invoke(ref message))
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

                case "nick":
                    if (auth)
                    {
                        def = command[1].Length == 0 ? Nick : command[1];
                        if (NickGroup.IndexOf(string.Concat(Delimiter, Nick, Delimiter)) == -1)
                        {
                            response = string.Concat("For security reasons I will only change my nick to the ones registered in my group:" +
                                Utility.Font.Underlined, NickGroup.Replace(Delimiter, Utility.Font.Underlined + " " + Utility.Font.Underlined));
                            notify = true;
                        }
                        else
                        {
                            nick = def;
                            client.send.WriteLine("NICK " + nick);
                        }
                    }
                    else
                    {
                        response = "You do not have the authority make me change my nick.";
                        notify = true;
                    }
                    break;

                case "uptime":
                case "cpu":
                    if (auth)
                        response = string.Concat(Utility.Font.Underlined + "Uptime:" + Utility.Font.Underlined, Utility.Process.Output("uptime"));
                    else
                    {
                        response = "You do not have the authority to request this information.";
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
                    var status = new StringBuilder(Client.BufferSize);
                    const string seperator = " | ";

                    status.Append("Uptime: ");
                    status.Append(Utility.Font.Bold);
                    status.Append(Utility.Time.ToDays(Math.Abs(Utility.Time.TimeSpanNow().Subtract(start).TotalSeconds)));
                    status.Append(Utility.Font.Bold);

                    status.Append(seperator);
                    status.Append(Utility.Font.Colour);
                    status.Append(Utility.Font.Colours.Brown);
                    status.Append("Memory: ");
                    status.Append(Utility.Font.Bold);
                    long memory = Process.GetCurrentProcess().PrivateMemorySize64;
                    status.Append(Math.Round((decimal)(memory / 1024 / 1024), 2).ToString());
                    status.Append(" MiB");
                    status.Append(Utility.Font.Bold);
                    status.Append(Utility.Font.Colour);

                    status.Append(seperator);
                    status.Append(Utility.Font.Colour);
                    status.Append(Utility.Font.Colours.Purple);
                    status.Append("Queries: ");
                    status.Append(Utility.Font.Bold);
                    status.Append(queries.ToString());
                    status.Append(Utility.Font.Bold);
                    status.Append(Utility.Font.Colour);

                    status.Append(seperator);
                    status.Append(Utility.Font.Colour);
                    status.Append(Utility.Font.Colours.Blue);
                    status.Append("Pastes: ");
                    status.Append(Utility.Font.Bold);
                    status.Append(pastes.ToString());
                    status.Append(Utility.Font.Bold);
                    status.Append(Utility.Font.Colour);

                    response = status.ToString();
                    break;

                case "hello":
                case "who":
                case "hey":
                case "sup":
                case "hi":
                    response = string.Format("Hi {0}, I'm a helper bot for #{1}. To find out more about me type /msg {2} help",
                        message.User.Nick, ChannelName, nick);
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
                    response = string.Format("Hello {0}, how can we help? If you have not already please read the tutorial at " +
                        "http://www.autohotkey.com/docs/Tutorial.htm", message.User.Nick);
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

                case "asl":
                    response = Utility.AgeSexLocation.Response();
                    break;

                case "o/":
                    response = "\\o";
                    action = true;
                    break;

                case "\\o":
                    response = "o/";
                    action = true;
                    break;

                case "mud":
                    if (command[2].Length == 0)
                        response = "kip";
                    break;

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

                case "rr":
#if RR
                    if (auth)
                    {
                        string mode = command[1].ToLowerInvariant();
                        switch (mode)
                        {
                            case "on":
                                Op = rr.Enabled = rrEnabled = true;
                                response = "Russian roulette is now enabled.";
                                break;

                            case "off":
                                Op = rr.Enabled = rrEnabled = false;
                                response = "Russian roulette is now disabled.";
                                RRDefaults(true);
                                break;

                            default:
                                response = "You are an operator of " + Utility.Font.Bold + Channel +
                                    Utility.Font.Bold + " so there is no need for you to play the odds :P";
                                break;
                        }
                        notify = true;
                    }
                    else if (message.Target != Channel)
                        response = string.Format("You need to be on the channel {0}{1}{0} to use this command.", Utility.Font.Bold, Channel);
                    else if (!rrEnabled)
                    {
                        response = "Sorry, Russian roulette is currently disabled. Please ask an operator to turn it on.";
                        notify = true;
                    }
                    else
                    {
                        RRStat rrtarget = new RRStat
                        {
                            Won = Utility.Texts.Random.Next(0, 50) == 13,
                            Time = Environment.TickCount,
                            User = message.User
                        };
                        if (rrtarget.Won)
                        {
                            client.Private(Client.ChanServ, "VOICE " + Channel + " " + message.User.Nick);
                            int mins = RRWon / RRTicks;
                            response = string.Format("Congratulations you have won {0} minute{1} of voice in {2} :D", mins, mins > 1 ? "s" : string.Empty, Channel);
                            notify = true;
                        }
                        else
                        {
                            client.Mode(Channel, "+" + RRBanFlag + " *!*@" + message.User.Host);
                            Thread.Sleep(SendMicroDelay);
                            client.Kick(Channel, message.User.Nick, "unlucky");
                            int mins = RRBan / RRTicks;
                            response = string.Format("Sorry, you have lost and will be banned for {0} minute{1} :(", mins, mins > 1 ? "s" : string.Empty);
                            notify = true;
                        }
                        rrList.Add(rrtarget);
                    }
#endif
#if !RR
                    response = "Sorry, Russian roulette is currently unavailable.";
                    notify = true;
#endif
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

                case "loli":
                case "onee":
                case "lion":
                case "stuff":
                case "lolistats":
                case "oneestats":
                case "lionstats":
                case "stuffstats":
                case "mio":
                    return;

                #endregion

                #region Search

                case "rss":
                case "r":
                    defs = Utility.Search.AutoHotkey.LatestRSS();
                    if (defs == null)
                    {
                        response = "Unable to retrieve RSS at this time.";
                        notify = true;
                    }
                    else
                    {
                        response = string.Format("Latest forum topic: {0}{1}{0} - {2}", Utility.Font.Bold, defs[0], defs[1]);
                        queries++;
                    }
                    break;

                case "define":
                case "def":
                case "d":
                    response = "Sorry, define is currently disabled.";
                    notify = true;
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
                        response = string.Format("{0}{7}{0} made {1}6{2}{3}{2}{1} posts; {2}{4}{2} - {5} {1}14 on {6}",
                            new string[] { Utility.Font.Underlined, Utility.Font.Colour, Utility.Font.Bold,
                                defs[0], defs[2], defs[1], defs[3], command[2] });
                        queries++;
                    }
                    break;

                case "forumstats":
                case "whobbs":
                    def = Utility.Search.AutoHotkey.ForumStats();
                    if (def.Length == 0)
                    {
                        response = "Unable to retrieve forum statistics at this time.";
                        notify = true;
                    }
                    else
                    {
                        response = def;
                        queries++;
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
                    {
                        response = string.Format("{0}{1}{0}: {2} - {3}",
                            new string[] { Utility.Font.Underlined, command[2], defs[0], defs[1] });
                        queries++;
                    }
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
                        queries++;
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
                    {
                        response = string.Format("{3}{0}{3}: {2} - {1}",
                            new string[] { defs[0], defs[1], defs[2], Utility.Font.Bold });
                        queries++;
                    }
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

                    for (int i = 0; i < Commands.Names.Length; i++)
                    {
                        if (Commands.Names[i].ToLower() == def)
                        {
                            response = string.Format("\x02{0}6{1}{0}\x02: http://www.autohotkey.com/{2}",
                                Utility.Font.Colour, Commands.Names[i], Commands.URIs[i]);
                            break;
                        }
                    }
                    if (response.Length == 0)
                    {
                        defs = Utility.Search.Google.AutoHotkey(def);
                        if (defs == null)
                        {
                            response = "Could not find " + Utility.Font.Bold + def + Utility.Font.Bold;
                            notify = true;
                        }
                        else
                        {
                            response = string.Format("Found \"{0}{1}{0}\": {2}", Utility.Font.Bold, defs[1], defs[0]);
                            queries++;
                        }
                    }
                    break;

                #endregion
            }

            #region Message

            if (message.Target == nick)
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
