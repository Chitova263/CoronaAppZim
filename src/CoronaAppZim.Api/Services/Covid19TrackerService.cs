using System;
using System.Threading;
using System.Threading.Tasks;
using Covid19.Client;
using Covid19.Client.Models;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Services
{
    public class Covid19TrackerService: ICovidTrackerService
    {
        private readonly ICovid19Client covid19Client;
        private readonly ILogger<Covid19TrackerService> logger;

        public Covid19TrackerService(ICovid19Client covid19Client, ILogger<Covid19TrackerService> logger)
        {
            this.covid19Client = covid19Client ?? throw new ArgumentNullException(nameof(covid19Client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default)
        {
            return this.covid19Client.GetLatestReportAsync(country, cancellationToken);
        }
    }
}
