using System;
using System.Collections;
using System.Text;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public event EventHandler<MessageEventArgs> MessageEvent;

        public class MessageEventArgs : EventArgs
        {
            public MessageEventArgs(Client client, Message message)
            {
                Client = client;
                Message = message;
            }

            public Client Client { get; protected set; }
            public Message Message { get; protected set; }
        }
    }
}
