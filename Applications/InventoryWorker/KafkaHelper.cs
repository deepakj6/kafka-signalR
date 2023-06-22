using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryWorker
{
    using Confluent.Kafka;
    using System;

    public class KafkaHelper
    {
        private readonly string bootstrapServers;
        private readonly string apiKey;
        private readonly string apiSecret;

        public KafkaHelper(string bootstrapServers, string apiKey, string apiSecret)
        {
            this.bootstrapServers = bootstrapServers;
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        public ConsumerBuilder<string, string> CreateConsumerBuilder()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = apiKey,
                SaslPassword = apiSecret,
                GroupId = "your_consumer_group_id"
            };

            return new ConsumerBuilder<string, string>(config);
        }

        public ProducerBuilder<string, string> CreateProducerBuilder()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = apiKey,
                SaslPassword = apiSecret
            };

            return new ProducerBuilder<string, string>(config);
        }
    }

}
