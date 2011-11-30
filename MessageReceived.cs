using System;
using robokins.IRC;

namespace robokins
{
    partial class Bot
    {
        public event EventHandler<MessageReceivedArgs> MessageReceived;

        public class MessageReceivedArgs : EventArgs
        {
            public MessageReceivedArgs(Client client, Message message)
            {
                Client = client;
                Message = message;
            }

            public Client Client { get; protected set; }
            public Message Message { get; protected set; }
        }
    }
}
