using System;
using System.Windows.Forms;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public Bot()
        {
            Application.ApplicationExit += new EventHandler(delegate(object sender, EventArgs e)
            {
                quit = true;
            });
        }

        public void Start()
        {
            if (reading)
                return;

            reading = true;
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

                    if (!Action(message))
                        Trigger(message);

                    if (quit)
                    {
#if PASTE
                        if (paste.Enabled)
                            paste.Stop();
#endif

#if RR
                        if (rr.Enabled)
                            rr.Stop();
                        RRDefaults(true);
#endif

#if LKINS
                        autoTriggers.Stop();
#endif

                        if (irc.Connected)
                        {
                            client.Quit("Got to go, bye!");
                            irc.Client.Close(SendDelay);
                        }
                        break;
                    }
                }
            }

            reading = false;
        }
    }
}
