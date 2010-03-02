using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Web;

namespace robokins.Utility
{
    class Twitter
    {
        public const int MaxLength = 140;
        const string update = "http://api.twitter.com/1/statuses/update.xml";
        const string delete = "http://api.twitter.com/1/statuses/destroy/{0}.xml";

        public static bool Delete(string username, SecureString password, string id)
        {
            var response = HTTP.DownloadPage(string.Format(delete, id), string.Empty, GetAuth(username, password));
            var confirm = Texts.StringBetween(response, "<id>", "</id>", 0);
            return confirm != null;
        }

        public static string Update(string username, SecureString password, string message)
        {
            if (message.Length > MaxLength)
                return null;

            var response = HTTP.DownloadPage(update, "status=" + HttpUtility.UrlEncode(message), GetAuth(username, password));
            var id = Texts.StringBetween(response, "<id>", "</id>", 0);

            return id;
        }

        static NetworkCredential GetAuth(string username, SecureString password)
        {
            var auth = new NetworkCredential() { UserName = username, Domain = string.Empty };
            IntPtr bstr = Marshal.SecureStringToBSTR(password);
            auth.Password = Marshal.PtrToStringUni(bstr);
            try { Marshal.ZeroFreeBSTR(bstr); }
            catch (MissingMethodException) { }
            return auth;
        }
    }
}
