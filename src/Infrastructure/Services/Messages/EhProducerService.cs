// ------------------------------------------------------------------------------------
// EHProducerService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Application.Dtos;
using Newtonsoft.Json;

namespace netca.Infrastructure.Services.Messages
{
    /// <summary>
    /// EHProducerService
    /// </summary>
    public class EhProducerService : IEhProducerService
    {
        private readonly ILogger<EhProducerService> _logger;
        private readonly IRedisService _redisService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EhProducerService"/> class.
        /// EHProducerService
        /// </summary>
        /// <param name="redisService"></param>
        /// <param name="logger"></param>
        public EhProducerService(IRedisService redisService, ILogger<EhProducerService> logger)
        {
            _logger = logger;
            _redisService = redisService;
        }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="az"></param>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SendAsync(AzureEventHub az, string topic, string message, CancellationToken cancellationToken)
        {
            var result = false;
            if (string.IsNullOrEmpty(message))
            {
                _logger.LogWarning("Failed to Produce Message, message Null");
            }
            else
            {
                try
                {
                    await using var producerClient = new EventHubProducerClient(az.ConnectionString, topic);
                    var data = new EventData(Encoding.UTF8.GetBytes(message));
                    using (var eventBatch = await producerClient.CreateBatchAsync(cancellationToken))
                    {
                        eventBatch.TryAdd(data);

                        await producerClient.SendAsync(eventBatch, cancellationToken);
                    }

                    result = true;

                    _logger.LogDebug("Sent to Topic: {Topic} Partition: {Partition} Offset: {Offset} Message: {Message}", topic, data.PartitionKey, data.Offset, message);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Produce Message, Saving to redis {Message}", ex.Message);
                    var dcaEhRedisDto = new EventHubRedisDto { Name = az.Name, Value = message };
                    await _redisService.ListLeftPushAsync(
                            Constants.RedisSubKeyMessageProduce + topic,
                            JsonConvert.SerializeObject(dcaEhRedisDto)
                        );
                }
            }

            return result;
        }
    }
}
