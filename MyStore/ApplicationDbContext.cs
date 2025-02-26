using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Configurarea relației Many-to-Many între Product și Size prin ProductSize
            modelBuilder.Entity<ProductSize>()
               .HasKey(ps => new { ps.ProductId, ps.SizeId });

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Size)
                .WithMany(s => s.ProductSizes)
                .HasForeignKey(ps => ps.SizeId);

            // Configurarea relației Many-to-One între Address și User
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            // Configurarea relației One-to-Many între Brand și Category
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Brand)
                .WithMany(b => b.Categories)
                .HasForeignKey(c => c.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurarea relației Many-to-One între Size și Category
            modelBuilder.Entity<Size>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Sizes)
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Promotion)
                .WithMany()  // A promotion applies to multiple products
                .HasForeignKey(p => p.PromotionId);
            //One-to-Many between Cart and CartItem:
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);
            //Many-to-One between CartItem and ProductSize
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.ProductSize)
                .WithMany() // ProductSize doesn't need a collection here
                .HasForeignKey(ci => ci.ProductSizeId);
            // Relationship between CartItem and Product
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)  // A CartItem has one Product
                .WithMany()  // A Product does not need a CartItem collection
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Change to SetNull if needed


            // Configurarea cheilor primare implicite din BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Setăm cheia primară pentru entitățile ce extind BaseEntity
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasKey(nameof(BaseEntity.Id));
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
