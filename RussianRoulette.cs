using System;
using System.Collections.Generic;
using System.Timers;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
#if RR
        class RRStat
        {
            public bool Won;
            public int Time;
            public User User;
        }

        Timer rr;
        bool rrEnabled = false;
        List<RRStat> rrList = new List<RRStat>();

        void RRDefaults(object sender, ElapsedEventArgs e)
        {
            RRDefaults(false);
        }

        void RRDefaults(bool force)
        {
            var actions = new List<string>();

            for (int i = 0; i < rrList.Count; i++)
            {
                if (force || (Environment.TickCount - rrList[i].Time >= (rrList[i].Won ? RRWon : RRBan)))
                {
                    if (rrList[i].Won)
                        client.Private(Client.ChanServ, "DEVOICE " + Channel + rrList[i].User.Nick);
                    else
                    {
                        client.Mode(Channel, "-" + RRBanFlag + " *!*@" + rrList[i].User.Host);
                        client.Invite(rrList[i].User.Nick, Channel);
                    }
                    rrList.RemoveAt(i);
                }
            }
        }
#endif
    }
}
