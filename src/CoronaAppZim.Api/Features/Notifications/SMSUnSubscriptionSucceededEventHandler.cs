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
    public class SMSUnSubscriptionSucceededEventHandler
        : INotificationHandler<SMSUnSubscriptionSucceededEvent>
    {
        private readonly ILogger<SMSUnSubscriptionSucceededEventHandler> logger;
        private readonly IOptionsMonitor<AWSSNSSettings> options;

        public SMSUnSubscriptionSucceededEventHandler(ILogger<SMSUnSubscriptionSucceededEventHandler> logger, IOptionsMonitor<AWSSNSSettings> options)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public async Task Handle(SMSUnSubscriptionSucceededEvent notification, CancellationToken cancellationToken)
        {
            var _snsClient = new AmazonSimpleNotificationServiceClient(
                this.options.CurrentValue.AWSAccessKeyId,
                this.options.CurrentValue.AwsSecretAccessKey,
                Amazon.RegionEndpoint.USEast1
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
                Message = "UnSubscription Successful",
                MessageAttributes = messageAttributes,
                PhoneNumber = notification.MobileNumber,
            };

            var publishResponse = await _snsClient.PublishAsync(pubRequest, cancellationToken);
            
            if(publishResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                this.logger.LogInformation($"--- sms sending successful {publishResponse.MessageId} ");
                return;
            }

            this.logger.LogError("--- sending message failed");

            return;
        }
    }
}