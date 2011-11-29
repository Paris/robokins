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
            send.Write(PING);
            send.Write(' ');
            send.WriteLine(Concat(server, " "));
            send.Flush();
        }

        public void Pong(params string[] server)
        {
            Pong(server);
        }

        public void Pong(IEnumerable<string> server)
        {
            send.Write(PONG);
            send.Write(' ');
            send.WriteLine(Concat(server, " "));
            send.Flush();
        }

        public void Error(string message)
        {
            send.Write(ERROR);
            send.Write(' ');
            send.WriteLine(message);
            send.Flush();
        }

        [Conditional("DEBUG")]
        public void Raw(string message)
        {
            send.WriteLine(message);
            send.Flush();
        }
    }
}
