using System.Collections.Generic;
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
    public class SMSSubscriptionSucceededEventHandler 
        : INotificationHandler<SMSSubscriptionSucceededEvent>
    {
        private readonly ILogger<SMSSubscriptionSucceededEventHandler> logger;
        private readonly IOptions<AWSSNSSettings> options;

        public SMSSubscriptionSucceededEventHandler(ILogger<SMSSubscriptionSucceededEventHandler> logger, IOptions<AWSSNSSettings> options)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public async Task Handle(SMSSubscriptionSucceededEvent notification, CancellationToken cancellationToken)
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
                Message = "Subscription Successful",
                MessageAttributes = messageAttributes,
                PhoneNumber = notification.MobileNumber,
            };

            //BUG https://github.com/jbogard/MediatR/issues/237
            //have a notification handler for EntityUpdated notification, which would ideally do as little work as possible, i.e. store it somewhere that we need to send the email -- this could be some sort of message queue, or just a database table, or you could schedule a background task (e. g. using Hangfire);
            //have a background job that handles all sms sending, which can, ideally, do retries when the email server is unavailable.
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