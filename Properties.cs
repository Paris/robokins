using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public SecureString Password { set; private get; }
    }
}
