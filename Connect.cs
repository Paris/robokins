using System.IO;
using System.Net.Sockets;
using System.Timers;
using robokins.IRC;
using Threading = System.Threading;

namespace robokins
{
    partial class Bot
    {
        void Connect()
        {
            irc = new TcpClient(Server, Port);
            irc.SendBufferSize = Client.BufferSize;
            irc.ReceiveBufferSize = Client.BufferSize;

            client = new Client(irc.GetStream());
            client.Pass(password);
            //password.Dispose();
            client.User(Username, InitUsermode, RealName);
            client.Private(Client.NickServ, "GHOST " + Nick);
            client.Nick(Nick);
            client.Mode(Nick, Usermode);
            
            Threading.Thread.Sleep(SendDelay * 3);

            client.Join(Channel, string.Empty);

#if PASTE
            paste = new Timer(PasteFreq);
            if (Directory.Exists(PasteSync))
            {
                pasteDir = new DirectoryInfo(PasteSync);
                foreach (FileInfo file in pasteDir.GetFiles())
                    file.Delete();
                paste.Elapsed += new ElapsedEventHandler(PasteCheck);
                paste.Start();
            }
#endif

#if LKINS || MIOKINS
            bots = new Timer(60 * 60 * 1000 / 2);
            bots.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
            {
#if LKINS
                Message("Lolikins", "!stuff");
#endif
#if MIOKINS
                Message("miokins", string.Concat("`mio ", random.Next(1, 6).ToString()));
#endif
            });
            bots.Start();
#endif
        }
    }
}
