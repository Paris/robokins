
namespace robokins.IRC
{
    partial class Client
    {
        public void Ping(params string[] server)
        {
            send.Write("PING ");
            send.WriteLine(string.Join(" ", server));
            send.Flush();
        }

        public void Pong(params string[] server)
        {
            send.Write("PONG ");
            send.WriteLine(string.Join(" ", server));
            send.Flush();
        }

        public void Error(string message)
        {
            send.Write("PONG ");
            send.WriteLine(message);
            send.Flush();
        }

#if DEBUG
        public void Raw(string message)
        {
            send.WriteLine(message);
            send.Flush();
        }
#endif
    }
}