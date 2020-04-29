using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CoronaAppZim.Api.Config;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoronaAppZim.Api.Features.Notifications
{
    public class SubscribeCommand
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
            public async Task<bool> Handle(Command command, CancellationToken cancellationToken)
            {
                var _snsClient = new AmazonSimpleNotificationServiceClient(
                    this.options.CurrentValue.AWSAccessKeyId,
                    this.options.CurrentValue.AwsSecretAccessKey,
                    Amazon.RegionEndpoint.USEast1
                );

                 var subscribeRequest = new SubscribeRequest
                {
                    TopicArn = this.options.CurrentValue.TopicArn,
                    Protocol = "sms",
                    Endpoint = command.MobileNumber,
                };

                var subscribeResponse = await _snsClient.SubscribeAsync(subscribeRequest, cancellationToken);

                if(subscribeResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    this.logger.LogInformation($"--- subscription successful");
                    return true;
                }

                this.logger.LogInformation($"--- subscription failed");

                return false;
            }
        }
    }
}