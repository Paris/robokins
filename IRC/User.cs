using System.Text;

namespace robokins.IRC
{
    class User
    {
        public string Nick;
        public string Ident;
        public string Host;

        public User(string alias)
        {
            int x = alias.IndexOf('!');
            Nick = x == -1 ? string.Empty : alias.Substring(0, x);

            x++;
            int y = alias.IndexOf('@', x);
            Ident = y == -1 ? string.Empty : alias.Substring(x, y - x);

            y++;
            Host = alias.Substring(y);
        }

        public override string ToString()
        {
            var buf = new StringBuilder(Nick.Length + Ident.Length + Host.Length + 2);
            if (Nick.Length != 0)
            {
                buf.Append(Nick);
                buf.Append('!');
            }
            if (Ident.Length != 0)
            {
                buf.Append(Ident);
                buf.Append('@');
            }
            buf.Append(Host);
            return buf.ToString();
        }
    }
}
