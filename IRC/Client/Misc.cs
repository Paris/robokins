using System.Diagnostics;

namespace robokins.IRC
{
    partial class Client
    {
        public void Ping(string server1, string server2 = null)
        {
            Send.Write(PING);
            Send.Write(' ');
            Send.Write(server1);
            if (!string.IsNullOrEmpty(server2))
            {
                Send.Write(' ');
                Send.Write(server2);
            }
            Send.WriteLine();
            Send.Flush();
        }

        public void Pong(string server1, string server2 = null)
        {
            Send.Write(PONG);
            Send.Write(' ');
            Send.Write(server1);
            if (!string.IsNullOrEmpty(server2))
            {
                Send.Write(' ');
                Send.Write(server2);
            }
            Send.WriteLine();
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
