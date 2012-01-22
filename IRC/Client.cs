using System;
using System.Diagnostics;
using System.IO;

namespace robokins.IRC
{
    partial class Client : IDisposable
    {
        public Stream ClientStream { get; protected set; }
        public StreamWriter Send { get; protected set; }

        bool quit;

        public event EventHandler Quitting;
        public event EventHandler<Message> MessageReceived;

        [Conditional("DEBUG")]
        void Echo(string text)
        {
            Console.Write(DateTime.Now.ToUniversalTime().ToString("[HH:mm:ss] "));
            Console.WriteLine(text);
        }

        public Client(Stream client)
        {
            ClientStream = client;
            Send = new StreamWriter(ClientStream);
            Send.NewLine = Linefeed;

            AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e) { Quit(string.Empty); };
        }

        public void Listen()
        {
            string line;

            using (var receive = new StreamReader(ClientStream))
            {
                while ((line = receive.ReadLine()) != null)
                {
                    Echo(line);
                    string[] msg = line.Split(new[] { ' ' }, 3);

                    if (msg[0] == Client.PING)
                    {
                        var target = msg[msg.Length > 2 ? 2 : 1];
                        if (target.Length > 1)
                            Pong(target[0] == ':' ? target.Substring(1) : target);
                    }
                    else if (msg[1] == Client.PRIVMSG)
                    {
                        Message message = null;
                        try { message = new Message(line); }
                        catch (ArgumentOutOfRangeException) { }

                        if (MessageReceived != null && message != null)
                            MessageReceived(this, message);

                        if (quit)
                        {
                            if (Quitting != null)
                                Quitting(this, new EventArgs());
                            break;
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            Send.Close();
            ClientStream.Close();
        }
    }
}
