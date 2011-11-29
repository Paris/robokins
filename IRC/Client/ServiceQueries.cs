
namespace robokins.IRC
{
    partial class Client
    {
        public void Servlist(string mask, string type)
        {
            Send.Write(SERVLIST);
            Send.Write(' ');
            Send.Write(mask);
            Send.Write(' ');
            Send.WriteLine(type);
            Send.Flush();
        }

        public void Squery(string service, string text)
        {
            Send.Write(SQUERY);
            Send.Write(' ');
            Send.Write(service);
            Send.Write(" :");
            Send.WriteLine(text);
            Send.Flush();
        }
    }
}
