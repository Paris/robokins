using System;
using System.Diagnostics;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public event EventHandler Quitting;

        public Bot()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(delegate(object sender, EventArgs e) { quit = true; });
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
                if (irc.Connected)
                {
                    client.Quit("Got to go, bye!");
                    irc.Client.Close(SendDelay);
                }
            });

            while ((line = client.Receive.ReadLine()) != null)
            {
                Echo(line);
                string[] msg = line.Split(boundary, 3);

                if (msg[0] == Client.PING)
                    client.Pong(msg[1]);
                else if (msg[1] == Client.PRIVMSG)
                {
                    Message message;
                    try { message = new Message(line); }
                    catch (ArgumentOutOfRangeException) { continue; }

                    MessageReceived(this, new MessageReceivedArgs(client, message));

                    if (quit)
                    {
                        Quitting(this, new EventArgs());
                        break;
                    }
                }
            }
        }
    }
}
