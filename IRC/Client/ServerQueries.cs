
namespace robokins.IRC
{
    partial class Client
    {
        public void Motd(string target)
        {
            Send.Write(INFO);
            Send.Write(" :");
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Lusers(string mask, string target)
        {
            Send.Write(LUSERS);
            Send.Write(' ');
            Send.Write(mask);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Version(string target)
        {
            Send.Write(VERSION);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Stats(string query, string target)
        {
            Send.Write(STATS);
            Send.Write(' ');
            Send.Write(query);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Links(string server, string mask)
        {
            Send.Write(LINKS);
            Send.Write(' ');
            Send.Write(server);
            Send.Write(' ');
            Send.WriteLine(mask);
            Send.Flush();
        }

        public void Time(string target)
        {
            Send.Write(TIME);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Connect(string server, string port, string remote)
        {
            Send.Write(CONNECT);
            Send.Write(' ');
            Send.Write(server);
            Send.Write(' ');
            Send.Write(port);
            Send.Write(' ');
            Send.WriteLine(remote);
            Send.Flush();
        }

        public void Trace(string target)
        {
            Send.Write(TRACE);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Admin(string target)
        {
            Send.Write(ADMIN);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Info(string target)
        {
            Send.Write(INFO);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Kill(string nickname, string comment)
        {
            Send.Write(KILL);
            Send.Write(' ');
            Send.Write(nickname);
            Send.Write(" :");
            Send.WriteLine(comment);
            Send.Flush();
        }
    }
}
