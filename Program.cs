using System;
using System.IO;
using System.Security;

namespace robokins
{
    partial class Program
    {
        public static void Main()
        {
            #region Password

            const string conf = Bot.Username + ".conf";
            if (!File.Exists(conf))
                throw new FileNotFoundException("Configuration file not found.", conf);

            var table = ConfRead(new StreamReader(conf));
            var passwd = new SecureString();
            const string key = "password";

            if (table.ContainsKey(key) && string.IsNullOrEmpty(table[key]))
            {
                foreach (char letter in table[key])
                    passwd.AppendChar(letter);
                passwd.MakeReadOnly();
                table.Remove(key);
            }

            if (passwd.Length == 0)
                throw new ArgumentNullException("Password is blank.");

            #endregion

            Tests(passwd);

            var bot = new Bot() { Password = passwd };
            bot.Start();
        }
    }
}
