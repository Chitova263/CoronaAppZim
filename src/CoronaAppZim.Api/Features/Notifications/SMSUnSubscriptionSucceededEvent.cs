using System;
using MediatR;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class SMSUnSubscriptionSucceededEvent: INotification
    {
        public Guid Id { get; set; }
        public string MobileNumber { get; set; }

        public SMSUnSubscriptionSucceededEvent(string mobileNumber)
        {
            Id = Guid.NewGuid();
            MobileNumber = mobileNumber;
        }
    }
}