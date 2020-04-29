using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CoronaAppZim.Api.Config;
using CoronaAppZim.Api.Features.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoronaAppZim.Api.Services
{
    public class AWSSNSService : INotificationService
    {
        private readonly AmazonSimpleNotificationServiceClient _snsClient;
        private readonly ILogger<AWSSNSService> logger;
        private readonly IOptionsMonitor<AWSSNSSettings> options;

        public AWSSNSService(ILogger<AWSSNSService> logger, IOptionsMonitor<AWSSNSSettings> options)
        {
            _snsClient = new AmazonSimpleNotificationServiceClient(
                this.options.CurrentValue.AWSAccessKeyId,
                this.options.CurrentValue.AwsSecretAccessKey,
                Amazon.RegionEndpoint.USEast1
                );
           
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
        }
        public async Task<bool> SendAsync(Message message, CancellationToken cancellationToken = default)
        {
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
                Message = message.Payload,
                MessageAttributes = messageAttributes,
                TopicArn = options.CurrentValue.TopicArn,
            };

            var publishResponse = await _snsClient.PublishAsync(pubRequest, cancellationToken);
            
            if(publishResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                this.logger.LogInformation($"--- sms sending successful {publishResponse.MessageId} ");
                return true;
            }

            this.logger.LogError("--- sending message failed");

            return false;
           
        }

        public async Task<bool> SubscribeAsync(Subscriber subscriber, CancellationToken cancellationToken = default)
        {
            var subscribeRequest = new SubscribeRequest
            {
                TopicArn = this.options.CurrentValue.TopicArn,
                Protocol = "sms",
                Endpoint = subscriber.MobileNumber,
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

        public Task<bool> UnSubscribeAsync(Subscriber subscriber, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}