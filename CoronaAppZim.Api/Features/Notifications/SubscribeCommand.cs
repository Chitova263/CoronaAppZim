using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CoronaAppZim.Api.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class SubscribeCommand
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
                this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
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

                var subscribeRequest = new SubscribeRequest
                {
                    TopicArn = this.options.Value.TopicArn,
                    Protocol = "sms",
                    Endpoint = command.MobileNumber,
                };

                var subscribeResponse = await _snsClient.SubscribeAsync(subscribeRequest, cancellationToken);

                if(subscribeResponse.HttpStatusCode == HttpStatusCode.OK)
                {

                    this.logger.LogInformation($"--- subscription successful @id: {subscribeResponse.ResponseMetadata.RequestId}");
                    
                    var @event = new SMSSubscriptionSucceededEvent(command.MobileNumber);
                    
                    await mediator.Publish(@event);

                    return Result.Success();
                }

                this.logger.LogInformation($"--- subscription failed");

                return Result.Fail("subscription failed");
            }
        }
    }
}