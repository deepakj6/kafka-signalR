namespace CartServer.Contracts.Requests
{
    public class CartItemRequest
    {
        public long CartId { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookCollectionId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
