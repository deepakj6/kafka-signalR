using CartServer.Contracts.Requests;
using CartServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CartServer.Respository
{
    public class CartDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public CartDbContext(DbContextOptions<CartDbContext> options)
            : base(options)
        {
            _ = Database.EnsureCreated();
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Carts");
                entity.HasKey(e => e.Id);

                entity.Property(c => c.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.UserId)
                    .IsRequired();

                entity.Property(c => c.TotalPrice)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(c => c.Discount)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValue(0);

                entity.HasMany(c => c.Items)
                    .WithOne()
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItems");
                entity.HasKey(ci => ci.Id);

                entity.Property(ci => ci.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(ci => ci.BookId)
                    .IsRequired();

                entity.Property(ci => ci.BookTitle)
                    .IsRequired();

                entity.Property(ci => ci.BookCollectionId)
                    .IsRequired();

                entity.Property(ci => ci.Quantity)
                    .IsRequired();

                entity.Property(ci => ci.Price)
                    .HasColumnType("decimal(18, 2)");
            });
        }*/
    }
}
