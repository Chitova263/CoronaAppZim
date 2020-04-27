using System.Threading;
using System.Threading.Tasks;
using Covid19.Client.Models;

namespace CoronaAppZim.Api.Services
{
    public interface ICovidTrackerService
    {
        Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default);
    }
}
