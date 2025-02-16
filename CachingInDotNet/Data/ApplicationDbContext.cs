//<copyright file="ApplicationDbContext" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using CachingInDotNet.models;
using Microsoft.EntityFrameworkCore;

namespace CachingInDotNet.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                productId = new Guid("393e2f50-9361-457f-80e5-c822e14f60fc"),
                productName = "Product 1",
                productDescription = "Product 1 Description",
                productPrice = 100.00,
                productQuantity = 10,
                productCategory = "Category 1",
                productCreatedDate = DateTime.SpecifyKind(new DateTime(2024, 01, 01, 12, 0, 0), DateTimeKind.Utc),
                ExpiryDateTime = DateTime.SpecifyKind(new DateTime(2024, 02, 01, 12, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                productId = new Guid("6f0f3f96-1c9b-41dd-bd5b-589a39f0a1ad"),
                productName = "Product 2",
                productDescription = "Product 2 Description",
                productPrice = 200.00,
                productQuantity = 20,
                productCategory = "Category 2",
                productCreatedDate = DateTime.SpecifyKind(new DateTime(2024, 01, 02, 12, 0, 0), DateTimeKind.Utc),
                ExpiryDateTime = DateTime.SpecifyKind(new DateTime(2024, 02, 02, 12, 0, 0), DateTimeKind.Utc)
            }
        );
    }
    //Always use static values in HasData() to avoid migration conflicts.
    //Use dynamic values (DateTime.UTCNow) in application logic, not in migrations.
    
}