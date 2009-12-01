using System.Diagnostics;
using System.Timers;

namespace robokins
{
    partial class Bot
    {
        Timer bots = null;

        [Conditional("FUNBOTS")]
        void FunBotsSetup()
        {
            var random = new System.Random();
            bots = new Timer(60 * 60 * 1000 / 2);
            bots.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
            {
                Message("Lolikins", "!stuff");
                Message("miokins", string.Concat("`mio ", random.Next(1, 6).ToString()));
            });
            bots.Start();
        }
    }
}
