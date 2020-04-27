using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Features.News;

namespace CoronaAppZim.Api.Services
{
    public interface INewsService
    {
        Task<Stories> GetStoriesAsync(string query, CancellationToken cancellationToken = default);
    }
}
