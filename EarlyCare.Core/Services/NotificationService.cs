using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using EarlyCare.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IConfiguration _configuration;

        public NotificationService(ILogger<NotificationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool SendEmail()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendMessage(string phoneNumber, string message)
        {
            try
            {
                var request = new PublishRequest
                {
                    PhoneNumber = $"+91{phoneNumber}",
                    Message = message
                };

                request.MessageAttributes.Add("AWS.SNS.SMS.SMSType", new MessageAttributeValue { StringValue = "Transactional", DataType = "String" });

                var credentials = new BasicAWSCredentials(_configuration["Aws:AccessKey"], _configuration["Aws:SecretKey"]);
                using (var client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.APSouth1))
                {
                    var response = await client.PublishAsync(request);
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        _logger.LogInformation($"Successfully sent SNS message '{response.MessageId}'");
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning(
                            $"Received a failure response '{response.HttpStatusCode}' when sending SNS message '{response.MessageId ?? "Missing ID"}'");
                        return false;
                    }
                };
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                _logger.LogError(ex, "An AWS SNS exception was thrown");
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was thrown");
                throw ex;
            }
        }
    }
}