using System.Text;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
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
