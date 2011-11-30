using System;
using System.Diagnostics;
using System.Timers;

namespace robokins
{
    partial class Bot
    {
        [Conditional("FUNBOTS")]
        void FunBotsSetup()
        {
            var timer = new Timer(60 * 60 * 1000 / 2);

            timer.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
            {
                Message(client, "lolikins", "!stuff");
            });

            Quitting += new EventHandler(delegate(object sender, EventArgs e)
            {
                if (timer.Enabled)
                    timer.Stop();
            });

            timer.Start();
        }
    }
}
