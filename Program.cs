using System;
using System.IO;
using System.Security;

namespace robokins
{
    partial class Program
    {
        public static void Main()
        {
            Tests();

            #region Password

            const string conf = Bot.Username + ".conf";
            if (!File.Exists(conf))
                throw new FileNotFoundException("Password file not found.", conf);

            var passwd = new SecureString();

            var stream = new StreamReader(conf);
            while (!stream.EndOfStream)
                passwd.AppendChar((char)stream.Read());
            passwd.MakeReadOnly();

            if (passwd.Length == 0)
                throw new ArgumentNullException("Password is blank.");

            #endregion

            var bot = new Bot();
            bot.Password = passwd;
            bot.Start();
        }
    }
}
