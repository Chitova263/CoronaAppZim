using System.Threading;
using System.Threading.Tasks;
using Covid19.Client;
using Covid19.Client.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace CoronaAppZim.Api.Features.Tracker
{
    public class GetLatestReportQuery
    {
        public class Request: IRequest<Result<Response>>
        {
            public string Country { get; set; }
        }

        public class Response
        {
            public int? Confirmed { get; set; }
            public int? Recovered { get; set; }
            public int? Deaths { get; set; }
            public DateTimeOffset Timestamp { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result<Response>>
        {
            private readonly ICovid19Client covid19Client;
            private readonly ILogger<Handler> logger;

            public Handler(ICovid19Client covid19Client, ILogger<Handler>logger)
            {
                this.covid19Client = covid19Client ?? throw new System.ArgumentNullException(nameof(covid19Client));
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            }
            public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var locations = await this.covid19Client.GetLocationsAsync();
                var country = locations.Locations
                    .FirstOrDefault(x => x.Country_Region.Trim().ToLowerInvariant() == request.Country.Trim().ToLowerInvariant());

                if (country is null)
                    return Result.Fail<Response>($"The requested country {request.Country} is not found");

                var response = await this.covid19Client.GetTimeSeriesAsync();

                var confirmed = response.ConfirmedTimeSeries
                    .FirstOrDefault(x => x.Country_Region == request.Country)
                    .TimeSeriesData
                    .Select(x => new
                    {
                        Date = x.Key,
                        Confirmed = x.Value,
                    })
                    .Last();

                var recovered = response.RecoveredTimeSeries
                    .FirstOrDefault(x => x.Country_Region == request.Country)
                    .TimeSeriesData
                    .Select(x => new
                    {
                        Date = x.Key,
                        Recovered = x.Value,
                    })
                    .Last();

                var deaths = response.DeathsTimeSeries
                   .FirstOrDefault(x => x.Country_Region == request.Country)
                   .TimeSeriesData
                   .Select(x => new
                   {
                       Date = x.Key,
                       Deaths = x.Value,
                   })
                   .Last();


                var result = new Response
                {
                    Confirmed = confirmed.Confirmed,
                    Recovered = recovered.Recovered,
                    Deaths = deaths.Deaths,
                    Timestamp = confirmed.Date,
                };

                return Result.Success(result);
            }
        }
    }
}