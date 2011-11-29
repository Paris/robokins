using System.Collections.Generic;

namespace robokins.IRC
{
    partial class Client
    {
        public void Away(string text)
        {
            Send.Write(AWAY);
            Send.Write(" :");
            Send.WriteLine(text);
            Send.Flush();
        }

        public void Rehash()
        {
            Send.WriteLine(REHASH);
            Send.Flush();
        }

        public void Die()
        {
            Send.WriteLine(DIE);
            Send.Flush();
        }

        public void Restart()
        {
            Send.WriteLine(RESTART);
            Send.Flush();
        }

        public void Summon(string user, string target, string channel)
        {
            Send.Write(SUMMON);
            Send.Write(' ');
            Send.Write(user);
            Send.Write(' ');
            Send.Write(target);
            Send.Write(' ');
            Send.WriteLine(channel);
            Send.Flush();
        }

        public void Users(string target)
        {
            Send.Write(USERS);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Wallops(string text)
        {
            Operwall(text);
        }

        public void Operwall(string text)
        {
            Send.Write(WALLOPS);
            Send.Write(" :");
            Send.WriteLine(text);
            Send.Flush();
        }

        public void Userhost(params string[] nickname)
        {
            Userhost(nickname);
        }

        public void Userhost(IEnumerable<string> nickname)
        {
            Send.Write(USERHOST);
            Send.Write(' ');
            Concat(nickname, " ", Send);
            Send.Flush();
        }

        public void Ison(params string[] nickname)
        {
            Ison(nickname);
        }

        public void Ison(IEnumerable<string> nickname)
        {
            Send.Write(ISON);
            Send.Write(' ');
            Concat(nickname, " ", Send);
            Send.Flush();
        }
    }
}
