using System.Collections.Generic;
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
    public class SendSMSCommand
    {
        public class Command: IRequest<CommandResult>
        {
            public string Payload { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommandResult>
        {
            private readonly ILogger<Handler> logger;
            private readonly IOptions<AWSSNSSettings> options;

            public Handler(ILogger<Handler> logger, IOptions<AWSSNSSettings> options)
            {
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
                this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            }
            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var _snsClient = new AmazonSimpleNotificationServiceClient(
                    this.options.Value.AWSAccessKeyId,
                    this.options.Value.AwsSecretAccessKey,
                    Amazon.RegionEndpoint.USWest2
                );

                var messageAttributes = new Dictionary<string, MessageAttributeValue>();
                var smsType = new MessageAttributeValue
                {
                    DataType = "String",
                    StringValue = "Transactional"
                };

                messageAttributes.Add("AWS.SNS.SMS.SMSType", smsType);

                this.logger.LogInformation("--- sending sms");

                var pubRequest = new PublishRequest
                {
                    Message = command.Payload,
                    MessageAttributes = messageAttributes,
                    TopicArn = options.Value.TopicArn,
                };

                var publishResponse = await _snsClient.PublishAsync(pubRequest, cancellationToken);
                
                if(publishResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    this.logger.LogInformation($"--- sms sending successful {publishResponse.MessageId} ");
                    return CommandResult.Success();
                }

                this.logger.LogError("--- sending message failed");

                return CommandResult.Fail(publishResponse.ResponseMetadata);
            }
        }
    }
}