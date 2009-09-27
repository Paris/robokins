using System;
using System.Text;
using System.Threading;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        bool Op
        {
            set { Message(Client.ChanServ, string.Format("{0}OP {1} {2}", value ? "" : "DE", Channel, Nick)); }
        }

        bool IsChannel(string target)
        {
            return target.Length > 1 && target[0] == '#';
        }

        void Message(string target, string text)
        {
            Message(target, text, false);
        }

        void Message(string target, string text, bool notice)
        {
            int since = Environment.TickCount - sent;
            if (since < SendDelay && since > 0)
                Thread.Sleep(since);

            if (notice)
                client.Notice(target, text);
            else
                client.Private(target, text);

            sent = Environment.TickCount;
        }

        string Action(string msg)
        {
            var buf = new StringBuilder(8 + msg.Length);
            buf.Append('\x01');
            buf.Append("ACTION ");
            buf.Append(msg);
            buf.Append('\x01');
            return buf.ToString();
        }
    }
}
