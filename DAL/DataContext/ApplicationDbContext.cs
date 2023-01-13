using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContext
{
    // identity dbcontext use  garera euta class taneko paxi aru pani tannu parxa
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
    {
        public ApplicationDbContext(DbContextOptions options ):base(options)
        { }  
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasKey(c => c.Id);
            //builder.Entity<Category>()
            //    .HasMany(c => c.Products)
            //    .WithOne(p => p.Category)
            //    .HasForeignKey(p => p.CategoryId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.Price).HasColumnType("decimal(18,4)");
                p.Property(x => x.Name).IsRequired().HasMaxLength(255);
                p.Property(x => x.Description).IsRequired().HasMaxLength(600);
            });
            builder.Entity<Product>()
                .HasMany(pc =>pc.ProductCategories)
                .WithOne(p => p.Product)
                .HasForeignKey(p =>p.ProductId)
                .IsRequired();

            builder.Entity<Category>()
                .HasMany(pc => pc.ProductCategories)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .IsRequired();
                
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

    }
}
    