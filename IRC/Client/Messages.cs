
namespace robokins.IRC
{
    partial class Client
    {
        public void Private(string target, string text)
        {
            Query(PRIVMSG, target, text);
        }

        public void Notice(string target, string text)
        {
            Query(NOTICE, target, text);
        }


        void Query(string type, string target, string text)
        {
            Send.Write(type);
            Send.Write(' ');
            Send.Write(target);
            Send.Write(" :");
            Send.WriteLine(text);
            Send.Flush();
        }
    }
}
