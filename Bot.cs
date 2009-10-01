using System;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public Bot()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(delegate(object sender, EventArgs e) { quit = true; });
        }

        public void Start()
        {
            Connect();
            string line;

            while ((line = client.receive.ReadLine()) != null)
            {
#if DEBUG
                Console.WriteLine(line);
#endif
                string[] msg = line.Split(boundary, 3);

                if (msg[0] == "PING")
                {
                    client.send.Write("PONG ");
                    client.send.WriteLine(msg[1]);
                }
                else if (msg[1] == "PRIVMSG")
                {
                    ReceivedMessage message;
                    try { message = new ReceivedMessage(line); }
                    catch (ArgumentOutOfRangeException) { continue; }

                    Trigger(message);

                    if (quit)
                    {
#if PASTE
                        if (paste.Enabled)
                            paste.Stop();
#endif

                        if (bots != null)
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
