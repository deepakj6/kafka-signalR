using System;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace InventoryServer.Models.Messages
{
    public class AddToCartStarted
    {
        public int Version { get; set; }
        public string CartId { get; set; }
        public string BookId { get; set; }
        public int Quantity { get; set; }

        public static void Configure()
        {
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = "https://confluent.cloud/environments/env-nw3y9z/schema-registry"
            };

            var schemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryConfig);

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "your-bootstrap-servers",
                // Other Kafka settings...
            };

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "your-bootstrap-servers",
                GroupId = "your-consumer-group-id",
                // Other Kafka settings...
            };


            var avroSerializer = new AvroSerializer<AddToCartStarted>(schemaRegistryClient, producerConfig);
            var avroDeserializer = new AvroDeserializer<AddToCartStarted>(schemaRegistryClient).AsSyncOverAsync();


/*            // Serialize
            var message = new YourAvroMessage { *//* Set message properties *//* };
            var serializedData = await avroSerializer.SerializeAsync(message);

            // Deserialize
            var deserializedMessage = await avroDeserializer.DeserializeAsync(serializedData);*/

           

            var serializerConfig = new ProducerConfig();

            //var avroSerializer = new AvroSerializer<AddToCartStarted>(schemaRegistry, schemaRegistryConfig);

            var producer = new ProducerBuilder<string, AddToCartStarted>(serializerConfig)
                .SetValueSerializer(avroSerializer.AsSyncOverAsync())
                .Build();
        }
    }
}
