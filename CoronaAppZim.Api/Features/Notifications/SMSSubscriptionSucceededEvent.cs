using System;
using MediatR;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class SMSSubscriptionSucceededEvent: INotification
    {
        public Guid Id { get; }
        public string MobileNumber { get; }

        public SMSSubscriptionSucceededEvent(string mobileNumber)
        {
            Id = Guid.NewGuid();
            MobileNumber = mobileNumber;
        }
    }
}