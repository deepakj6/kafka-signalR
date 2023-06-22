using System;
using System.Text;
using Confluent.Kafka;

public class InventoryEventProducer
{
    private readonly string bootstrapServers;

    public InventoryEventProducer(string bootstrapServers)
    {
        this.bootstrapServers = bootstrapServers;
    }

    public void PublishInventoryEvent(string inventoryEventJson)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };

        using (var producer = new ProducerBuilder<string, string>(config).Build())
        {
            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = inventoryEventJson
            };

            try
            {
                producer.Produce("inventory_updates", message);
                Console.WriteLine($"Published inventory event: {inventoryEventJson}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Error publishing inventory event: {ex.Error.Reason}");
            }
        }
    }
}
