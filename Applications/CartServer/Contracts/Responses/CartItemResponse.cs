namespace CartServer.Contracts.Responses
{
    public class CartItemResponse
    {
        public long Id { get; set; }
        public long CartId { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookCollectionId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
