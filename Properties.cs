using System;
using System.Net.Sockets;
using System.Security;
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

        public SecureString Password
        {
            set { password = value; }
        }
    }
}
