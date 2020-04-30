using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class SMSSubscriptionSucceededEventHandler : INotificationHandler<SMSSubscriptionSucceededEvent>
    {
        public Task Handle(SMSSubscriptionSucceededEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}