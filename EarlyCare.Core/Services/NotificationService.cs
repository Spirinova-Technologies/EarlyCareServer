using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using EarlyCare.Core.Interfaces;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task<bool> SendEmail(EmailModel emailModel)
        {
            // Change to your region
            var credentials = new BasicAWSCredentials(_configuration["Aws:AccessKey"], _configuration["Aws:SecretKey"]);
            using (var client = new AmazonSimpleEmailServiceClient(credentials, Amazon.RegionEndpoint.APSouth1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = emailModel.FromEmail,
                    Destination = new Destination
                    {
                        ToAddresses = emailModel.ToEmailAddresses
                    },
                    Message = new Message
                    {
                        Subject = new Content(emailModel.Subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = emailModel.Body
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = emailModel.Body
                            }
                        }
                    }
                };
                 await client.SendEmailAsync(sendRequest);
            }

            return true;
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