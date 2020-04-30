using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
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
        }

        public class Handler : IRequestHandler<Command, CommandResult>
        {
            private readonly IMediator mediator;
            private readonly ILogger<Handler> logger;
            private readonly IOptionsMonitor<AWSSNSSettings> options;

            public Handler(IMediator mediator, ILogger<Handler> logger, IOptionsMonitor<AWSSNSSettings> options)
            {
                this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
                this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var _snsClient = new AmazonSimpleNotificationServiceClient(
                    this.options.CurrentValue.AWSAccessKeyId,
                    this.options.CurrentValue.AwsSecretAccessKey,
                    Amazon.RegionEndpoint.USWest2
                );

                var subscriptions = await _snsClient.ListSubscriptionsByTopicAsync(this.options.CurrentValue.TopicArn, cancellationToken);
                var subscriptionARN = subscriptions.Subscriptions
                    .FirstOrDefault(x => x.Endpoint == command.MobileNumber);

                var response = await _snsClient.UnsubscribeAsync(subscriptionARN.SubscriptionArn, cancellationToken);

                if(response.HttpStatusCode == HttpStatusCode.OK)
                {
                    this.logger.LogInformation($"--- unsubscription successful");

                    var @event = new SMSUnSubscriptionSucceededEvent(command.MobileNumber);
                    
                    await this.mediator.Publish(@event);

                    return CommandResult.Success();
                }

                this.logger.LogInformation($"--- unsubscription unsuccessful");

                return CommandResult.Fail(response.ResponseMetadata);

            }
        }
    }
}