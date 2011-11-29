using System;
using System.Runtime.InteropServices;
using System.Security;

namespace robokins.IRC
{
    partial class Client
    {
        public void Pass(SecureString password)
        {
            Send.Write(PASS);
            Send.Write(' ');
            IntPtr bstr = Marshal.SecureStringToBSTR(password);
            Send.WriteLine(Marshal.PtrToStringUni(bstr));
            try { Marshal.ZeroFreeBSTR(bstr); }
            catch (MissingMethodException) { }
            Send.Flush();
        }

        public void Nick(string nickname)
        {
            Send.Write(NICK);
            Send.Write(' ');
            Send.WriteLine(nickname);
            Send.Flush();
        }

        public void User(string user, string mode, string realname)
        {
            Send.Write(USER);
            Send.Write(' ');
            Send.Write(user);
            Send.Write(' ');
            Send.Write(mode);
            Send.Write(" * :");
            Send.WriteLine(realname);
            Send.Flush();
        }

        public void Oper(string name, SecureString password)
        {
            Send.Write("OPER ");
            Send.Write(name);
            Send.Write(' ');
            IntPtr bstr = Marshal.SecureStringToBSTR(password);
            Send.WriteLine(Marshal.PtrToStringUni(bstr));
            Marshal.ZeroFreeBSTR(bstr);
            Send.Flush();
        }

        public void Service(string nickname, string distribution, string info)
        {
            Send.Write(SERVICE);
            Send.Write(' ');
            Send.Write(nickname);
            Send.Write(" * ");
            Send.Write(distribution);
            Send.Write(" 0 0 :");
            Send.WriteLine(info);
            Send.Flush();
        }

        public void Quit(string message)
        {
            Send.Write(QUIT);
            Send.Write(" :");
            Send.WriteLine(message);
            Send.Flush();
        }

        public void Squit(string server, string comment)
        {
            Send.Write(SQUIT);
            Send.Write(' ');
            Send.Write(server);
            Send.Write(" :");
            Send.WriteLine(comment);
            Send.Flush();
        }
    }
}
