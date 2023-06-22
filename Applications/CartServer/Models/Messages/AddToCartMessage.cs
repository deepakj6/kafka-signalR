namespace CartServer.Models.Messages
{
    public class AddToCartStartedMessage
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookCollectionId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Version { get; set; }
    }

}
