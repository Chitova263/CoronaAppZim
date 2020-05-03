using System.ComponentModel.DataAnnotations;

namespace CoronaAppZim.Api.Configuration
{
    public class AWSSNSSettings
    {
        //configuration validation
        [Required]
        public string AWSAccessKeyId { get; set; }
        [Required]
        public string AwsSecretAccessKey { get; set; }
        [Required]
        public string TopicArn { get; set; }

    }
}

