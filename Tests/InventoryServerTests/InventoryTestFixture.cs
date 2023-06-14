namespace InventoryServerTests
{
    public class InventoryTestFixture : IDisposable
    {
        public DbContextOptions<InventoryDbContext> DbContextOptions { get; private set; }
        public InventoryDbContext DbContext { get; private set; }

        public InventoryTestFixture()
        {
            // Set up the test database connection using SQLite in-memory database
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            DbContextOptions = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseSqlite(connection)
                .Options;

            DbContext = new InventoryDbContext(DbContextOptions);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }
}
