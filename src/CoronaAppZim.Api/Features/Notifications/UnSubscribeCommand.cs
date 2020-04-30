using System;
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
        public class Command: IRequest<CommandResult>
        {
            public string MobileNumber { get; set; }
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommandResult>
        {
            private readonly ILogger<Handler> logger;
            private readonly IOptionsMonitor<AWSSNSSettings> options;

            public Handler(ILogger<Handler> logger, IOptionsMonitor<AWSSNSSettings> options)
            {
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
                this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            }

            public Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}