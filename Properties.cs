using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        bool quit = false;

        public SecureString Password { set; private get; }
    }
}
