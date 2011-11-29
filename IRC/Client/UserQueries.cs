using System.Collections.Generic;

namespace robokins.IRC
{
    partial class Client
    {
        public void Who(string mask, bool op)
        {
            send.Write(WHO);
            send.Write(' ');
            send.Write(mask);
            send.WriteLine(op ? " o" : string.Empty);
            send.Flush();
        }

        public void Whois(string target, string mask)
        {
            send.Write(WHOIS);
            send.Write(' ');
            send.Write(target);
            send.Write(' ');
            send.WriteLine(mask);
            send.Flush();
        }

        public void Whowas(IEnumerable<string> nickname, string count, string target)
        {
            send.Write(WHOWAS);
            send.Write(' ');
            Concat(nickname, ",", send);
            send.Write(' ');
            send.Write(count);
            send.Write(' ');
            send.WriteLine(target);
            send.Flush();
        }

        public void Whowas(string nickname, string count, string target)
        {
            send.Write(WHOWAS);
            send.Write(' ');
            send.Write(nickname);
            send.Write(' ');
            send.Write(count);
            send.Write(' ');
            send.WriteLine(target);
            send.Flush();
        }
    }
}
