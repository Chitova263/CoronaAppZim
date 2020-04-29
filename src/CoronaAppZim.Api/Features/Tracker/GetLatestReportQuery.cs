using System.Threading;
using System.Threading.Tasks;
using Covid19.Client;
using Covid19.Client.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Features.Tracker
{
    public class GetLatestReportQuery
    {
        public class Request: IRequest<Response>
        {
            public string Country { get; set; }
        }

        public class Response
        {
            public LatestReport LatestReport { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ICovid19Client covid19Client;
            private readonly ILogger<Handler> logger;

            public Handler(ICovid19Client covid19Client, ILogger<Handler>logger)
            {
                this.covid19Client = covid19Client ?? throw new System.ArgumentNullException(nameof(covid19Client));
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var response = await this.covid19Client.GetLatestReportAsync(request.Country, cancellationToken);
                return new Response 
                {
                    LatestReport = response
                };
            }
        }
    }
}