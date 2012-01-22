using System;
using System.IO;
using System.Security;
using robokins.Utility;

namespace robokins
{
    partial class Program
    {
        public static void Main()
        {
            #region Keys

            const string conf = Bot.Username + ".conf";
            var table = ConfRead(new StreamReader(conf));

            #region Password

            if (!File.Exists(conf))
                throw new FileNotFoundException("Configuration file not found.", conf);

            var passwd = new SecureString();
            const string key = "password";

            if (table.ContainsKey(key) && !string.IsNullOrEmpty(table[key]))
            {
                foreach (char letter in table[key])
                    passwd.AppendChar(letter);
                passwd.MakeReadOnly();
                table.Remove(key);
            }

            if (passwd.Length == 0)
                throw new ArgumentNullException("Password is blank.");

            #endregion

            #region Others

            const string bitlyauth = "bitlyauth", bitlykey = "bitlykey";

            if (table.ContainsKey(bitlyauth))
                Links.BitlyAuth = table[bitlyauth];
            if (table.ContainsKey(bitlykey))
                Links.BitlyKey = table[bitlykey];

            #endregion

            #endregion

            Tests(passwd);

            var bot = new Bot { Password = passwd };
            bot.Connect();
        }
    }
}
