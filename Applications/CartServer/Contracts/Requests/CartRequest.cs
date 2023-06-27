namespace CartServer.Contracts.Requests
{
    public class CartRequest
    {
        public string UserId { get; set; }
        public List<CartItemRequest> Items { get; set; } = new List<CartItemRequest>();
        public decimal TotalPrice { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
    }
}
