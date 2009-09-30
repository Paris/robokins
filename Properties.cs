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
        public static readonly char[] boundary = new char[] { ' ' };
        int sent = 0;
        string nick;
        bool quit = false;
        bool reading = false;
        TimeSpan start = Utility.Time.TimeSpanNow();
        SecureString password;
        Random random = new Random();

        public bool Stopped
        {
            get { return quit; }
        }

        public SecureString Password
        {
            get { return password; }
            set { password = value; }
        }

#if LKINS || MIOKINS
        Timer bots;
#endif
    }
}
