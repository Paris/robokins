using System;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        bool Invoke(ref ReceivedMessage message)
        {
            message.Text = message.Text.Trim();

            if (message.User.Host == "services." || message.User.Ident == "freenode" || message.User.Nick.Length == 0 || message.Text.Length == 0)
                return false;

#if LKINS
            if (message.User.Nick.Equals(Lolikins, StringComparison.OrdinalIgnoreCase))
                return false;
#endif

            bool query = message.Target[0] != '#';
            char first = message.Text[0];
            string word = message.Text.Split(Bot.boundary, 2, StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
            string nickLow = nick.ToLowerInvariant();

            if (first == '!' || first == '@')
            {
                int remove = 1;
                word = word.Substring(1);
                if (word.Equals(ChannelName, StringComparison.OrdinalIgnoreCase) || word == nickLow)
                    remove += word.Length;
                message.Text = message.Text.Substring(remove).Trim();
                return message.Text.Length != 0;
            }
            else if (word.IndexOf(nickLow) == 0)
            {
                bool range = nick.Length < word.Length;
                bool bound = range ? !char.IsLetterOrDigit(word, nick.Length) : false;
                if (!range || (range && bound))
                {
                    message.Text = message.Text.Substring(nick.Length + (bound ? 1 : 0)).Trim();
                    return message.Text.Length != 0;
                }
                else
                    return query;
            }
            else
                return query;
        }
    }
}
