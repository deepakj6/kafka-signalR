namespace InventoryServerTests
{
    public class InventoryRepositoryTests : IClassFixture<InventoryTestFixture>
    {
        private readonly DbContextOptions<InventoryDbContext> _dbContextOptions;
        private readonly InventoryDbContext _dbContext;
        public InventoryRepositoryTests(InventoryTestFixture fixture)
        {
            _dbContextOptions = fixture.DbContextOptions;
            _dbContext = fixture.DbContext;
        }

        #region Book CRUD Tests

        [Fact]
        public void AddBook_BookIsAddedSuccessfully()
        {
            // Arrange
            string sampleBookId = Guid.NewGuid().ToString();
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);
                var book = CreateTestBook(sampleBookId, sampleBookCollectionId);

                // Act
                repository.AddBookCollection(bookCollection);
                repository.AddBook(book);

                // Assert
                var addedBook = dbContext.Books.Find(book.Id);
                Assert.NotNull(addedBook);
                Assert.Equal(book.Title, addedBook.Title);
                Assert.Equal(book.Author, addedBook.Author);

                //Cleanup
                repository.DeleteBook(book.Id);
                repository.DeleteBookCollection(bookCollection.Id);

            }
        }

        [Fact]
        public void GetBook_BookExists_ReturnsBook()
        {
            // Arrange
            string sampleBookId = Guid.NewGuid().ToString();
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);

                // Add a book to the database
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);
                var book = CreateTestBook(sampleBookId, sampleBookCollectionId);
                dbContext.Books.Add(book);
                dbContext.SaveChanges();

                // Act
                var bookToFindId=book.Id;
                var retrievedBook = repository.GetBook(bookToFindId);

                // Assert
                Assert.NotNull(retrievedBook);
                Assert.Equal(book.Id, retrievedBook.Id);
                Assert.Equal(book.Title, retrievedBook.Title);
                Assert.Equal(book.Author, retrievedBook.Author);

                //Cleanup
                repository.DeleteBook(book.Id);
                repository.DeleteBookCollection(bookCollection.Id);
            }
        }

        [Fact]
        public void UpdateBook_BookExists_BookIsUpdatedSuccessfully()
        {
            // Arrange
            string sampleBookId = Guid.NewGuid().ToString();
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);

                // Add a book to the database
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);
                var book = CreateTestBook(sampleBookId, sampleBookCollectionId);
                dbContext.Books.Add(book);
                dbContext.SaveChanges();

                var updatedBook = book;                
                updatedBook.Title = "Updated Book Title";
                updatedBook.Author = "Updated Book Author";
                updatedBook.Description = "Updated Book Description";
                updatedBook.Price = 19.99;
                updatedBook.Quantity = 5;

                // Act
                repository.UpdateBook(updatedBook);

                // Assert
                var retrievedBook = dbContext.Books.Find(updatedBook.Id);
                Assert.NotNull(retrievedBook);
                Assert.Equal(updatedBook.Title, retrievedBook.Title);
                Assert.Equal(updatedBook.Author, retrievedBook.Author);


                //Cleanup
                repository.DeleteBook(book.Id);
                repository.DeleteBookCollection(bookCollection.Id);

            }
        }

        [Fact]
        public void DeleteBook_BookExists_BookIsDeletedSuccessfully()
        {
            // Arrange
            string sampleBookId = Guid.NewGuid().ToString();
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);

                // Add a book to the database
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);
                var book = CreateTestBook(sampleBookId, sampleBookCollectionId);
                var bookToDeleteId= book.Id;
                dbContext.Books.Add(book);
                dbContext.SaveChanges();

                // Act
                repository.DeleteBook(bookToDeleteId);

                // Assert
                var deletedBook = dbContext.Books.Find(bookToDeleteId);
                Assert.Null(deletedBook);

                //Cleanup                
                repository.DeleteBookCollection(bookCollection.Id);
            }
        }

        #endregion

        #region BookCollection CRUD Tests

        [Fact]
        public void AddBookCollection_BookCollectionIsAddedSuccessfully()
        {
            // Arrange
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);

                // Act
                repository.AddBookCollection(bookCollection);

                // Assert
                var addedBookCollection = dbContext.BookCollections.Find(bookCollection.Id);
                Assert.NotNull(addedBookCollection);
                Assert.Equal(bookCollection.Name, addedBookCollection.Name);

                // Cleanup
                repository.DeleteBookCollection(bookCollection.Id);
            }
        }

        [Fact]
        public void GetBookCollection_BookCollectionExists_ReturnsBookCollection()
        {
            // Arrange
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);

                // Add a book collection to the database
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);
                dbContext.BookCollections.Add(bookCollection);
                dbContext.SaveChanges();

                // Act
                var retrievedBookCollection = repository.GetBookCollection(bookCollection.Id);

                // Assert
                Assert.NotNull(retrievedBookCollection);
                Assert.Equal(bookCollection.Id, retrievedBookCollection.Id);
                Assert.Equal(bookCollection.Name, retrievedBookCollection.Name);

                // Cleanup
                repository.DeleteBookCollection(bookCollection.Id);
            }
        }

        [Fact]
        public void UpdateBookCollection_BookCollectionExists_BookCollectionIsUpdatedSuccessfully()
        {
            // Arrange
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);

                // Add a book collection to the database
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);
                dbContext.BookCollections.Add(bookCollection);
                dbContext.SaveChanges();

                var updatedBookCollection = bookCollection;
                updatedBookCollection.Name = "Updated Book Collection";

                // Act
                repository.UpdateBookCollection(updatedBookCollection);

                // Assert
                var retrievedBookCollection = dbContext.BookCollections.Find(updatedBookCollection.Id);
                Assert.NotNull(retrievedBookCollection);
                Assert.Equal(updatedBookCollection.Name, retrievedBookCollection.Name);

                // Cleanup
                repository.DeleteBookCollection(bookCollection.Id);
            }
        }

        [Fact]
        public void DeleteBookCollection_BookCollectionExists_BookCollectionIsDeletedSuccessfully()
        {
            // Arrange
            string sampleBookCollectionId = Guid.NewGuid().ToString();

            using (var dbContext = new InventoryDbContext(_dbContextOptions))
            {
                IInventoryRepository repository = new SqlInventoryRepository(dbContext);

                // Add a book collection to the database
                var bookCollection = CreateTestBookCollection(sampleBookCollectionId);
                dbContext.BookCollections.Add(bookCollection);
                dbContext.SaveChanges();

                // Act
                repository.DeleteBookCollection(bookCollection.Id);

                // Assert
                var deletedBookCollection = dbContext.BookCollections.Find(bookCollection.Id);
                Assert.Null(deletedBookCollection);
            }
        }


        #endregion

        #region Helper Methods

        private BookCollection CreateTestBookCollection(string sampleBookCollectionId)
        {            
            return new BookCollection
            {
                Id = sampleBookCollectionId,
                Name = "New Releases"
            };                        
        }
        private Book CreateTestBook(string sampleBookId, string sampleBookCollectionId)
        {
            return new Book
            {
                Id = sampleBookId,
                Title = "Book Title",
                Author = "Book Author",
                Description = "Book Description",
                Price = 9.99,
                Quantity = 10,
                BookCollectionId = sampleBookCollectionId
            };
        }

        #endregion
    }
}
