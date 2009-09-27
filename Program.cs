#define TESTS

using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;

namespace robokins
{
    partial class Program
    {
        public static void Main()
        {
#if DEBUG && TESTS
            Tests();
            return;
#endif

            const int RetryDelay = 5000;
            const int MaxRetry = 1 * 60 * 60 * 1000 / RetryDelay;

            const string pid = Bot.Username + ".pid";
            if (File.Exists(pid))
                File.Delete(pid);
            File.WriteAllText(pid, Process.GetCurrentProcess().Id.ToString());

            const string conf = Bot.Username + ".conf";
            if (!File.Exists(conf))
                throw new FileNotFoundException("Password file not found.", conf);

            var passwd = new SecureString();
            var stream = new StreamReader(conf);
            while (!stream.EndOfStream)
                passwd.AppendChar((char)stream.Read());
            passwd.MakeReadOnly();

            if (passwd.Length == 0)
                throw new Exception("Password is blank.");

            var bot = new Bot();
            bot.Password = passwd;

            for (int i = 0; i < MaxRetry; i++)
            {
                try
                {
                    bot.Start();
                }
                catch (IOException) { }

                if (bot.Stopped)
                    break;
                else
                {
                    Console.Error.WriteLine("Connection lost");
                    Thread.Sleep(RetryDelay);
                }
            }
        }
    }
}
