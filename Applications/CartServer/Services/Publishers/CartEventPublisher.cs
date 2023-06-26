using Confluent.Kafka;
using Newtonsoft.Json;

namespace CartServer.Services.Publishers
{
    public class CartEventPublisher : ICartEventPublisher
    {
        private readonly IProducer<string, string> _producer;

        public CartEventPublisher(IProducer<string, string> producer)
        {
            _producer = producer;
        }

        public void PublishAddToCartStarted(string bookId, string bookTitle, string bookCollectionId, int quantity, decimal price)
        {
            var eventData = new
            {
                BookId = bookId,
                BookTitle = bookTitle,
                BookCollectionId = bookCollectionId,
                Quantity = quantity,
                Price = price
            };

            var message = new Message<string, string>
            {
                Key = "add_to_cart_started",
                Value = JsonConvert.SerializeObject(eventData)
            };

            _producer.ProduceAsync("cart_events", message);
        }
    }

}
