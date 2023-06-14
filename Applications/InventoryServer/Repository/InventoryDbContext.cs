using InventoryServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using InventoryServer.Services.Repository;

namespace InventoryServer.Repository
{    
    public class InventoryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCollection> BookCollections { get; set; }       

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
            // Call EnsureCreated and seed data initialization
            EnsureDatabaseCreatedAndSeedData(); 
        }

       private void EnsureDatabaseCreatedAndSeedData()
        {
            if (Database.EnsureCreated())
            {
                // Database was created, initialize seed data
                SeedDatabase();
            }
        }

        private void SeedDatabase()
        {
            var inventory = SeedData.InitializeData();
            Books.AddRange(inventory.Books);
            BookCollections.AddRange(inventory.BookCollections);
            SaveChanges();
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Author).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Quantity);

                entity.Property(e => e.BookCollectionId).HasMaxLength(255); ;
            });

            modelBuilder.Entity<BookCollection>(entity =>
            {
                entity.ToTable("BookCollections");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired();
            });            
        }

    }

}
