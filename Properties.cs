using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public static readonly char[] boundary = new char[] { ' ' };
        bool quit = false;

        public SecureString Password { set; private get; }
    }
}
