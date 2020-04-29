using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Config;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class UnSubscribeCommand
    {
        public class Command: IRequest<bool>
        {
            public string MobileNumber { get; set; }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly ILogger<Handler> logger;
            private readonly IOptionsMonitor<AWSSNSSettings> options;

            public Handler(ILogger<Handler> logger, IOptionsMonitor<AWSSNSSettings> options)
            {
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
                this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            }

            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}