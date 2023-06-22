using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryWorker
{
    using System;
    using Confluent.Kafka;

    public class CartEventConsumer
    {
        private readonly string bootstrapServers;
        private readonly string consumerGroup;

        public CartEventConsumer(string bootstrapServers, string consumerGroup)
        {
            this.bootstrapServers = bootstrapServers;
            this.consumerGroup = consumerGroup;
        }

        public void StartConsuming()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = consumerGroup,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe("cart_events");

                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();

                        // Process the consumed cart event
                        ProcessCartEvent(consumeResult.Message.Value);
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Error consuming cart event: {ex.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing cart event: {ex.Message}");
                    }
                }
            }
        }

        private void ProcessCartEvent(string cartEventJson)
        {
            // TODO: Implement the logic to handle cart events
            // Here, you can update the inventory count based on the cart event
            // and perform validation checks

            Console.WriteLine($"Received cart event: {cartEventJson}");
        }
    }

}
