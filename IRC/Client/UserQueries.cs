using System.Collections.Generic;

namespace robokins.IRC
{
    partial class Client
    {
        public void Who(string mask, bool op)
        {
            Send.Write(WHO);
            Send.Write(' ');
            Send.Write(mask);
            Send.WriteLine(op ? " o" : string.Empty);
            Send.Flush();
        }

        public void Whois(string target, string mask)
        {
            Send.Write(WHOIS);
            Send.Write(' ');
            Send.Write(target);
            Send.Write(' ');
            Send.WriteLine(mask);
            Send.Flush();
        }

        public void Whowas(IEnumerable<string> nickname, string count, string target)
        {
            Send.Write(WHOWAS);
            Send.Write(' ');
            Concat(nickname, ",", Send);
            Send.Write(' ');
            Send.Write(count);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }

        public void Whowas(string nickname, string count, string target)
        {
            Send.Write(WHOWAS);
            Send.Write(' ');
            Send.Write(nickname);
            Send.Write(' ');
            Send.Write(count);
            Send.Write(' ');
            Send.WriteLine(target);
            Send.Flush();
        }
    }
}
