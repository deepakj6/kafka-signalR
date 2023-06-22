namespace InventoryServer.Models.Messages
{
    public class CartEventMessage
    {
        public int Version { get; set; }
        public string CartId { get; set; }
        public string BookId { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return "Version = " + Version.ToString() + "  Cart id = " + CartId.ToString() + "  BookId = " + BookId.ToString() + "  Quantity = " + Quantity.ToString();
        }
    }

}
