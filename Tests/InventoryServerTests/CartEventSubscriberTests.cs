using Confluent.Kafka;
using InventoryServer.Models.Messages;
using Microsoft.Extensions.Logging;

namespace InventoryServerTests
{
    public class CartEventSubscriberTests
    {
        private readonly ILogger<CartEventSubscriberTests> _logger;

        public CartEventSubscriberTests()
        {
            _logger = new LoggerFactory().CreateLogger<CartEventSubscriberTests>();
        }

        [Fact]
        public void Consume_All_Events_From_Topic_Cart_Events()
        {
            var config = new ConsumerConfig
            {
                //Note: Do not hardcode the username and passwords.
                //For testing, edit these properties before running the tests.
                BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
                SaslUsername = "XGSVY4BDG6NQHNLH",
                SaslPassword = "PWdtjHepHAD2RILbDq8H+v3wxS3jSuOWfGI/YKjKqlPS1cHCf+k7DH96oWBShxb2",
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
    }
}
