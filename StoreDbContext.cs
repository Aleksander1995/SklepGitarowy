using Microsoft.EntityFrameworkCore;

namespace SklepGitarowy
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=sklep.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Gitara akustyczna Yamaha", Price = 999.99m, Quantity = 5 },
                new Product { Id = 2, Name = "Gitara elektryczna Ibanez", Price = 1499.99m, Quantity = 3 },
                new Product { Id = 3, Name = "Wzmacniacz Fender", Price = 799.99m, Quantity = 4 },
                new Product { Id = 4, Name = "Struny D'Addario", Price = 39.99m, Quantity = 10 },
                new Product { Id = 5, Name = "Pasek gitarowy Ernie Ball", Price = 59.99m, Quantity = 7 }
            );
        }
    }
}
