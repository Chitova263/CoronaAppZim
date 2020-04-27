using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CoronaAppZim.Api.Features.Notifications;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Services
{
    public class AWSSNSService : INotificationService
    {
        private readonly AmazonSimpleNotificationServiceClient _snsClient;
        private readonly ILogger<AWSSNSService> logger;

        public AWSSNSService(ILogger<AWSSNSService> logger)
        {
            _snsClient = new AmazonSimpleNotificationServiceClient(
                "AKIATI4IWOOTD62SXCVL",
                "XsiZtFvlv6iCcxBr3PAMikVgSvHr3qw4mg7Cd1+g",
                Amazon.RegionEndpoint.USEast1
                );
           
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
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
                TopicArn = "arn:aws:sns:us-east-1:225235596198:coronazim-updates",
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
                TopicArn = "arn:aws:sns:us-east-1:225235596198:coronazim-updates",
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