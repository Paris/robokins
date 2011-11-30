using System.IO;

namespace robokins.IRC
{
    partial class Client
    {
        public StreamWriter Send { get; protected set; }
        public StreamReader Receive { get; protected set; }

        public Client(Stream client)
        {
            Send = new StreamWriter(client);
            Send.NewLine = Linefeed;
            Receive = new StreamReader(client);
        }
    }
}
