using System;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string Payload { get; set; }

        public Message()
        {
            MessageId = Guid.NewGuid();
        }
    }
}