using System;
using System.Text;

namespace robokins.IRC
{
    class Message
    {
        public Message(string query)
        {
            if (query.Length > Client.BufferSize || query.Length < (":x!y " + Client.NOTICE + " z :").Length || query[0] != ':')
                throw new ArgumentOutOfRangeException();

            int z;

            // RFC example:
            //   :Angel!wings@irc.org PRIVMSG Wiz :Are you receiving this message ?

            z = query.IndexOf(':', 1);
            if (z == -1)
                throw new ArgumentOutOfRangeException();
            Text = z + 1 >= query.Length ? string.Empty : query.Substring(z + 1);

            string[] values = query.Substring(1, z - 2).Split(' ');

            if (values.Length != 3)
                throw new ArgumentOutOfRangeException();

            User = new User(values[0]);

            switch (values[1].ToUpperInvariant())
            {
                case Client.PRIVMSG:
                    Notice = false;
                    break;
                case Client.NOTICE:
                    Notice = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Target = values[2];
        }

        public bool Notice { get; protected set; }

        public User User { get; protected set; }

        public string Target { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            var buf = new StringBuilder(Client.BufferSize);
            char bound = Bot.boundary[0];
            buf.Append(':');
            buf.Append(User.ToString());
            buf.Append(bound);
            buf.Append(Notice ? Client.NOTICE : Client.PRIVMSG);
            buf.Append(bound);
            buf.Append(Target);
            buf.Append(bound);
            buf.Append(':');
            buf.Append(Text);
            if (buf.Length > Client.BufferSize)
                throw new ArgumentOutOfRangeException();
            return buf.ToString();
        }
    }
}
