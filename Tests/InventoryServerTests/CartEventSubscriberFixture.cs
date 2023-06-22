using Confluent.Kafka;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryServerTests
{
    public class CartEventSubscriberFixture
    {
        public IConsumer<string, string> Consumer { get; private set; }

        public CartEventSubscriberFixture()
        {
            // Setup the Kafka consumer and any required dependencies
            Consumer = SetupConsumer();
        }

        private IConsumer<string, string> SetupConsumer()
        {
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Subscribe(It.IsAny<string>()));
            mockConsumer.Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                .Returns((ConsumeResult<string, string> result) => new ConsumeResult<string, string>[] { result });

            return mockConsumer.Object;
        }

        public void MessageHandler(Message<string, string> message, Action<string> handler)
        {
            // Call the handler method for the received message value
            handler(message.Value);
        }

    }
}
