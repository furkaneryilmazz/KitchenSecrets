using Microsoft.EntityFrameworkCore;

namespace KitchenSecrets.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-JK4TV70;Database=KitchenSecretsDB; User Id=sa;Password=123456;TrustServerCertificate=true");
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product - User ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // OnDelete davranışını Restrict olarak ayarladık

            // Comment - User ilişkisi
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Eğer User silindiğinde Comment'lerin de silinmesini istiyorsanız Cascade kullanabilirsiniz

            // Product - Category ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); // Eğer Category silindiğinde Product'ların CategoryId'sinin NULL olmasını istiyorsanız SetNull kullanabilirsiniz

            base.OnModelCreating(modelBuilder);
        }

    }
}
