using System;
using System.Diagnostics;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        protected bool Quit { get; set; }

        public event EventHandler Quitting;

        public Bot()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(delegate(object sender, EventArgs e) { Quit = true; });
        }

        [Conditional("DEBUG")]
        void Echo(string text)
        {
            Console.Write(DateTime.Now.ToUniversalTime().ToString("[HH:mm:ss] "));
            Console.WriteLine(text);
        }

        public void Start()
        {
            Connect();
            string line;

            MessageReceived += new EventHandler<MessageReceivedArgs>(Trigger);

            Quitting += new EventHandler(delegate(object sender, EventArgs e)
            {
                if (IrcStream.Connected)
                {
                    Client.Quit("Got to go, bye!");
                    IrcStream.Client.Close(SendDelay);
                }
            });

            while ((line = Client.Receive.ReadLine()) != null)
            {
                Echo(line);
                string[] msg = line.Split(new[] { ' ' }, 3);

                if (msg[0] == Client.PING)
                    Client.Pong(msg[1]);
                else if (msg[1] == Client.PRIVMSG)
                {
                    Message message;
                    try { message = new Message(line); }
                    catch (ArgumentOutOfRangeException) { continue; }

                    MessageReceived(this, new MessageReceivedArgs(Client, message));

                    if (Quit)
                    {
                        Quitting(this, new EventArgs());
                        break;
                    }
                }
            }
        }
    }
}
