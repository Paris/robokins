using System;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        bool Action(ReceivedMessage message)
        {
            const string action = "\x01" + "ACTION ";

            if (message.Text.IndexOf(action) != 0)
                return false;

            if (message.Text.Length < action.Length + nick.Length + 5 || !IsChannel(message.Target))
                return true;

            string[] parts = message.Text.Substring(action.Length).Split(boundary, 2);

            int z = parts[1].Length;

            if (z == 0)
                return true;

            z--;
            if (parts[1][z] == '\x01')
                parts[1] = parts[1].Substring(0, z);

            if (!string.Equals(parts[1].Trim(), nick, StringComparison.OrdinalIgnoreCase))
                return true;

            switch (parts[0].Trim().ToLowerInvariant())
            {
                case "slap":
                case "slaps":
                    client.Kick(message.Target, message.User.Nick, "oh yeah?");
                    break;
                    
                case "pat":
                case "pats":
                case "patpat":
                case "patpats":
                    Message(message.Target, Action("purrs"));
                    break;
            }

            return true;
        }
    }
}