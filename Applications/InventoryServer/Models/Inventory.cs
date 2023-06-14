namespace InventoryServer.Models
{
    public class Inventory
    {
        public List<Book> Books { get; set; }
        public List<BookCollection> BookCollections { get; set; }
    }

}
