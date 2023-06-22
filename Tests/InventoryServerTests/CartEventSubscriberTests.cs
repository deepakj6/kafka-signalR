using Confluent.Kafka;
using InventoryServer.Models.Messages;
using Microsoft.Extensions.Logging;

namespace InventoryServerTests
{
    public class CartEventSubscriberTests : IClassFixture<CartEventSubscriberFixture>
    {
        private readonly CartEventSubscriberFixture _fixture;
        private readonly ILogger<KafkaTesting> _logger;

        public CartEventSubscriberTests(CartEventSubscriberFixture fixture)
        {
            _fixture = fixture;
            _logger = new LoggerFactory().CreateLogger<KafkaTesting>();
        }

        [Fact]
        public void ConsumeSingleEventFromKafka()
        {
            var config = new ConsumerConfig
            {
                //Note: Do not hardcode the username and passwords.
                //For testing, edit these properties before running the tests.
                BootstrapServers = "",
                SaslUsername = "",
                SaslPassword = "",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                GroupId = "proxy:101" + Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            try
            {
                consumer.Subscribe("cart_events");
                bool toReadMessage = true;

                while (toReadMessage)
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(10));
                    _logger.LogInformation("This is where I put a breakpoint and res is returning NULL");
                    if (result is not null)
                    {
                        var cartData = Newtonsoft.Json.JsonConvert.DeserializeObject<CartEventMessage>(result.Value);

                        _logger.LogInformation(cartData.ToString());
                        ProcessEvent(result.Key, cartData);
                    }
                    else
                    {
                        toReadMessage = false;
                    }
                }
                consumer.Commit();

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

        }

        private void HandleException(Error error)
        {
            // Handle the error as needed
            _logger.LogInformation($"Kafka error: {error.Reason}");
        }

        private void ProcessEvent(string key, CartEventMessage value)
        {
            // Process the consumed event
            _logger.LogInformation($"Received event. Key: {key}, Value: {value}");
        }
    }
}
