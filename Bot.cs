using System;
using System.Collections.Generic;
using System.Diagnostics;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
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

            MessageEvent += new EventHandler<MessageEventArgs>(Trigger);

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

                    MessageEvent(this, new MessageEventArgs(client, message));

                    if (quit)
                    {
                        if (paste != null && paste.Enabled)
                            paste.Stop();

                        if (bots != null && bots.Enabled)
                            bots.Stop();

                        if (irc.Connected)
                        {
                            client.Quit("Got to go, bye!");
                            irc.Client.Close(SendDelay);
                        }
                        break;
                    }
                }
            }
        }
    }
}
