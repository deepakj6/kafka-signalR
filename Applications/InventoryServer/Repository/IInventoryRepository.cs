using InventoryServer.Models;

namespace InventoryServer.Repository
{
    public interface IInventoryRepository
    {
        Inventory GetInventory();
        Book GetBook(string id);
        IEnumerable<Book> GetAllBooks();
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(string id);
        BookCollection GetBookCollection(string id);
        IEnumerable<BookCollection> GetAllBookCollections();
        void AddBookCollection(BookCollection bookCollection);
        void UpdateBookCollection(BookCollection bookCollection);
        void DeleteBookCollection(string id);

    }
}
