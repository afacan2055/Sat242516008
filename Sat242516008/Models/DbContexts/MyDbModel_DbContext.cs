using Microsoft.EntityFrameworkCore;
using MyDbModels;

namespace DbContexts;

public class MyDbModel_DbContext(DbContextOptions<MyDbModel_DbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Ürün Eþleþtirmesi
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
        });

        // 2. Kategori Eþleþtirmesi
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.Id);
        });

        // 3. Kullanýcý Eþleþtirmesi (Kritik Bölüm)
        // Görseldeki tablo yapýsýna göre kolon isimlerini ve anahtarý netleþtiriyoruz
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users"); // SQL'deki tablo adýyla birebir ayný
            entity.HasKey(e => e.Id);

            // Eðer veritabanýnda kolon isimleri farklýysa (Örn: UserName) 
            // burada .HasColumnName("Username") ile eþitleyebilirsin.
        });

        // Madde 10: Ýliþkisel Yapý
        modelBuilder.Entity<Product>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}