using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Features.Notifications;

namespace CoronaAppZim.Api.Services
{
    public interface INotificationService
    {
        Task<bool> SendAsync(Message message, CancellationToken cancellationToken = default);
        Task<bool> SubscribeAsync(Subscriber subscriber, CancellationToken cancellationToken = default);
        Task<bool> UnSubscribeAsync(Subscriber subscriber, CancellationToken cancellationToken = default);
    }
}
