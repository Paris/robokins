using System;
using System.Net.Sockets;
using System.Security;
using System.Threading;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public SecureString Password { set; private get; }

        public void Connect()
        {
            using (var irc = new TcpClient(Server, Port))
            {
                irc.SendBufferSize = Client.BufferSize;
                irc.ReceiveBufferSize = Client.BufferSize;

                using (var client = new Client(irc.GetStream()))
                {
                    client.Pass(Password);
                    client.User(Environment.UserName, InitUsermode, RealName);
                    client.Nick(Nick);
                    client.Mode(Nick, Usermode);

                    Thread.Sleep(SendDelay * 3);

                    client.Join(Channel, string.Empty);

                    client.MessageReceived += Trigger;

                    PasteSetup(client);
                    FunBotsSetup(client);

                    client.Listen();
                }

                irc.Client.Close(SendDelay);
            }
        }
    }
}
