namespace CartServer.Services.Publishers
{
    public interface ICartEventPublisher
    {
        void PublishAddToCartStarted(string bookId, string bookTitle, string bookCollectionId, int quantity, decimal price);
        // Other event publishing methods...
    }

}
