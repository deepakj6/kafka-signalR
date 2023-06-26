using CartServer.Models;
using CartServer.Services.Publishers;
using Confluent.Kafka;
using FluentAssertions;
using Newtonsoft.Json;


namespace CartServerTests
{
    public class CartEventPublisherTests
    {
        [Fact]

        public async Task Publish_Single_AddToCartStarted_To_Topic_Cart_Events()
        {
            string topic, bookId, bookTitle, bookCollectionId, expectedKey, expectedValue;
            int quantity;
            decimal price;
            Publish_AddToCartStarted(out topic, out bookId, out bookTitle, out bookCollectionId, out quantity, out price, out expectedKey, out expectedValue);

            // Simulate a delay to allow the message to be produced (optional)
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Assert
            IConsumer<string, string> consumer;
            ConsumeResult<string, string>? consumeResult;
            Consume_Single_AddToCartStarted(topic, out consumer, out consumeResult);

            consumeResult.Should().NotBeNull();
            consumeResult.Message.Key.Should().Be(expectedKey);
            consumeResult.Message.Value.Should().Be(expectedValue);


            if (consumeResult is not null)
            {
                var cartData = JsonConvert.DeserializeObject<CartItem>(consumeResult.Value);
                cartData.Should().NotBeNull();
                cartData.BookId.Should().Be(bookId);
                cartData.BookTitle.Should().Be(bookTitle);
                cartData.BookCollectionId.Should().Be(bookCollectionId);
                cartData.Quantity.Should().Be(quantity);
                cartData.Price.Should().Be(price);
            }

        }

        private static void Consume_Single_AddToCartStarted(string topic, out IConsumer<string, string> consumer, out ConsumeResult<string, string>? consumeResult)
        {
            ConsumerConfig consumerConfig = GetConsumerConfig();
            consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            consumer.Subscribe(topic);
            consumeResult = consumer.Consume(TimeSpan.FromSeconds(10));
            consumer.Commit(consumeResult);
        }

        private static void Publish_AddToCartStarted(out string topic, out string bookId, out string bookTitle, out string bookCollectionId, out int quantity, out decimal price, out string expectedKey, out string expectedValue)
        {
            // Arrange
            ProducerConfig producerConfig = GetProducerConfig();

            var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            topic = "cart_events";
            bookId = "100";
            bookTitle = "Learning CSharp";
            bookCollectionId = "100";
            quantity = 1;
            price = 9.99m;
            expectedKey = "add_to_cart_started";
            expectedValue = JsonConvert.SerializeObject(new
            {
                BookId = bookId,
                BookTitle = bookTitle,
                BookCollectionId = bookCollectionId,
                Quantity = quantity,
                Price = price
            });

            // Act
            var publisher = new CartEventPublisher(producer);
            publisher.PublishAddToCartStarted(bookId, bookTitle, bookCollectionId, quantity, price);
        }

        private static ConsumerConfig GetConsumerConfig()
        {
            var consumerConfig = new ConsumerConfig
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
            return consumerConfig;
        }

        private static ProducerConfig GetProducerConfig()
        {
            var producerConfig = new ProducerConfig
            {
                // Note: Do not hardcode the username and password.
                // For testing, edit these properties before running the tests.
                BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
                SaslUsername = "XGSVY4BDG6NQHNLH",
                SaslPassword = "PWdtjHepHAD2RILbDq8H+v3wxS3jSuOWfGI/YKjKqlPS1cHCf+k7DH96oWBShxb2",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain
            };
            return producerConfig;
        }
    }
}