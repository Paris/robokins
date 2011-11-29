using System.Collections.Generic;
using System.Diagnostics;

namespace robokins.IRC
{
    partial class Client
    {
        public void Ping(params string[] server)
        {
            Ping(server);
        }

        public void Ping(IEnumerable<string> server)
        {
            Send.Write(PING);
            Send.Write(' ');
            Send.WriteLine(Concat(server, " "));
            Send.Flush();
        }

        public void Pong(params string[] server)
        {
            Pong(server);
        }

        public void Pong(IEnumerable<string> server)
        {
            Send.Write(PONG);
            Send.Write(' ');
            Send.WriteLine(Concat(server, " "));
            Send.Flush();
        }

        public void Error(string message)
        {
            Send.Write(ERROR);
            Send.Write(' ');
            Send.WriteLine(message);
            Send.Flush();
        }

        [Conditional("DEBUG")]
        public void Raw(string message)
        {
            Send.WriteLine(message);
            Send.Flush();
        }
    }
}
