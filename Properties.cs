using System;
using System.Net.Sockets;
using System.Security;
using System.Timers;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        TcpClient irc;
        Client client;
        SecureString password;
        public static readonly char[] boundary = new char[] { ' ' };
        int sent = 0;
        bool quit = false;

        TimeSpan start = Utility.Time.TimeSpanNow();
        Timer bots = null;

        public SecureString Password
        {
            set { password = value; }
        }
    }
}
