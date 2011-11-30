using System;
using System.Net.Sockets;
using System.Threading;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        protected TcpClient IrcStream { get; private set; }
        protected Client Client { get; private set; }

        void Connect()
        {
            IrcStream = new TcpClient(Server, Port);
            IrcStream.SendBufferSize = Client.BufferSize;
            IrcStream.ReceiveBufferSize = Client.BufferSize;

            Client = new Client(IrcStream.GetStream());
            Client.Pass(Password);
            Client.User(Environment.UserName, InitUsermode, RealName);
            Client.Nick(Nick);
            Client.Mode(Nick, Usermode);
            
            Thread.Sleep(SendDelay * 3);

            Client.Join(Channel, string.Empty);

            PasteSetup();
            FunBotsSetup();
        }
    }
}
