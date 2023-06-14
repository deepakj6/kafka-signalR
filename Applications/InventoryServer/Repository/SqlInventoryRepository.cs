using InventoryServer.Models;

namespace InventoryServer.Repository
{
    public class SqlInventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _dbContext;

        public SqlInventoryRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Inventory GetInventory()
        {
            var books = _dbContext.Books.ToList();
            var bookCollections = _dbContext.BookCollections.ToList();

            return new Inventory
            {
                Books = books,
                BookCollections = bookCollections
            };
        }

        public Book GetBook(string id)
        {
            return _dbContext.Books.Find(id);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _dbContext.Books.ToList();
        }

        public void AddBook(Book book)
        {
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _dbContext.Books.Update(book);
            _dbContext.SaveChanges();
        }

        public void DeleteBook(string id)
        {
            var book = _dbContext.Books.Find(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
            }
        }

        public BookCollection GetBookCollection(string id)
        {
            return _dbContext.BookCollections.Find(id);
        }

        public IEnumerable<BookCollection> GetAllBookCollections()
        {
            return _dbContext.BookCollections.ToList();
        }

        public void AddBookCollection(BookCollection bookCollection)
        {
            _dbContext.BookCollections.Add(bookCollection);
            _dbContext.SaveChanges();
        }

        public void UpdateBookCollection(BookCollection bookCollection)
        {
            _dbContext.BookCollections.Update(bookCollection);
            _dbContext.SaveChanges();
        }

        public void DeleteBookCollection(string id)
        {
            var bookCollection = _dbContext.BookCollections.Find(id);
            if (bookCollection != null)
            {
                _dbContext.BookCollections.Remove(bookCollection);
                _dbContext.SaveChanges();
            }
        }
    }

}
