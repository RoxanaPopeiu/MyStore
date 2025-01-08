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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Configurarea relației Many-to-Many între Product și Size prin ProductSize
            modelBuilder.Entity<ProductSize>()
                .HasKey(ps => ps.Id);

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
                .HasForeignKey(c => c.BrandId);

            // Configurarea relației Many-to-One între Size și Category
            modelBuilder.Entity<Size>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Sizes)
                .HasForeignKey(s => s.CategoryId);

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
