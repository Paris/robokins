using System;
using System.Text;
using System.Threading;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        static int sent = -1;

        static void Message(Client client, string target, string text, bool notice = false)
        {
            int since = Environment.TickCount - sent;
            if (since < SendDelay && since != -1)
                Thread.Sleep(since);

            if (notice)
                client.Notice(target, text);
            else
                client.Private(target, text);

            sent = Environment.TickCount;
        }

        static string Action(string msg)
        {
            const char boundary = '\x01';
            var buffer = new StringBuilder(2 + Client.ACTION.Length + msg.Length);
            buffer.Append(boundary);
            buffer.Append(Client.ACTION);
            buffer.Append(' ');
            buffer.Append(msg);
            buffer.Append(boundary);
            return buffer.ToString();
        }
    }
}
