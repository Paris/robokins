using System;
using System.Text;

namespace robokins.IRC
{
    class ReceivedMessage
    {
        public bool Notice;
        public User User;
        public string Target;
        public string Text;

        const string privmsg = "PRIVMSG";
        const string notice = "NOTICE";

        public ReceivedMessage(string query)
        {
            if (query.Length > Client.BufferSize || query.Length < (":x!y " + notice + " z :").Length || query[0] != ':')
                throw new ArgumentOutOfRangeException();

            int z;

            // RFC example:
            //   :Angel!wings@irc.org PRIVMSG Wiz :Are you receiving this message ?

            z = query.IndexOf(':', 1);
            if (z == -1)
                throw new ArgumentOutOfRangeException();
            Text = z + 1 >= query.Length ? string.Empty : query.Substring(z + 1);

            string[] values = query.Substring(1, z - 2).Split(Bot.boundary);

            if (values.Length != 3)
                throw new ArgumentOutOfRangeException();

            User = new User(values[0]);

            switch (values[1].ToUpperInvariant())
            {
                case privmsg:
                    Notice = false;
                    break;
                case notice:
                    Notice = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Target = values[2];
        }

        public override string ToString()
        {
            var buf = new StringBuilder(Client.BufferSize);
            char bound = Bot.boundary[0];
            buf.Append(':');
            buf.Append(User.ToString());
            buf.Append(bound);
            buf.Append(Notice ? notice : privmsg);
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
