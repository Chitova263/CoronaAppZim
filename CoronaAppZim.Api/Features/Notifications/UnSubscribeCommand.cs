using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using CoronaAppZim.Api.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class UnSubscribeCommand
    {
        public class Command: IRequest<Result>
        {
            public string MobileNumber { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IMediator mediator;
            private readonly ILogger<Handler> logger;
            private readonly IOptions<AWSSNSSettings> options;

            public Handler(IMediator mediator, ILogger<Handler> logger, IOptions<AWSSNSSettings> options)
            {
                this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
                this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var _snsClient = new AmazonSimpleNotificationServiceClient(
                    this.options.Value.AWSAccessKeyId,
                    this.options.Value.AwsSecretAccessKey,
                    Amazon.RegionEndpoint.USWest2
                );

                var subscriptions = await _snsClient.ListSubscriptionsByTopicAsync(this.options.Value.TopicArn, cancellationToken);
                var subscriptionARN = subscriptions.Subscriptions
                    .FirstOrDefault(x => x.Endpoint == command.MobileNumber);

                var response = await _snsClient.UnsubscribeAsync(subscriptionARN.SubscriptionArn, cancellationToken);

                if(response.HttpStatusCode == HttpStatusCode.OK)
                {
                    this.logger.LogInformation($"--- unsubscription successful");

                    var @event = new SMSUnSubscriptionSucceededEvent(command.MobileNumber);
                    
                    await this.mediator.Publish(@event);

                    return Result.Success();
                }

                this.logger.LogInformation($"--- unsubscription unsuccessful");

                return Result.Fail("unsubscription unsuccessful");

            }
        }
    }
}