using System;
using System.Text;
using System.Threading;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        void Message(string target, string text, bool notice = false)
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
            buf.Append(Client.ACTION);
            buf.Append(' ');
            buf.Append(msg);
            buf.Append('\x01');
            return buf.ToString();
        }
    }
}
