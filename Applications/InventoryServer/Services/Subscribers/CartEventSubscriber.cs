using System;
using Confluent.Kafka;
using InventoryServer.Repository;

namespace InventoryServer.Services.Subscribers
{

    public class CartEventSubscriber : ICartEventSubscriber
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IInventoryRepository _inventoryRepository;

        public CartEventSubscriber(IConsumer<string, string> consumer, IInventoryRepository inventoryRepository)
        {
            _consumer = consumer;
            _inventoryRepository = inventoryRepository;
        }

        public void SubscribeToCartUpdateStarted()
        {
            SubscribeToEvent("cart_update_started", HandleCartUpdateStarted);
        }

        public void SubscribeToAddToCartStarted()
        {
            SubscribeToEvent("add_to_cart_started", HandleAddToCartStarted);
        }

        public void SubscribeToRemoveFromCartStarted()
        {
            SubscribeToEvent("remove_from_cart_started", HandleRemoveFromCartStarted);
        }

        private void SubscribeToEvent(string eventName, Action<string> eventHandler)
        {
            _consumer.Subscribe("cart_events");

            // Start consuming messages in a background thread
            var consumingTask = Task.Run(() =>
            {
                while (true)
                {
                    var result = _consumer.Consume(new CancellationToken());

                    if (result.Message.Key == eventName)
                    {
                        eventHandler(result.Message.Value);
                    }
                }
            });
        }

        private void HandleCartUpdateStarted(string quantity)
        {
            var book = _inventoryRepository.GetBook("book_id"); // Replace "book_id" with the actual book ID
            book.Quantity = int.Parse(quantity);
            _inventoryRepository.UpdateBook(book);
        }

        private void HandleAddToCartStarted(string quantity)
        {
            var book = _inventoryRepository.GetBook("book_id"); // Replace "book_id" with the actual book ID
            book.Quantity -= int.Parse(quantity);
            _inventoryRepository.UpdateBook(book);
        }

        private void HandleRemoveFromCartStarted(string quantity)
        {
            var book = _inventoryRepository.GetBook("book_id"); // Replace "book_id" with the actual book ID
            book.Quantity += int.Parse(quantity);
            _inventoryRepository.UpdateBook(book);
        }
    }


}
