using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Management.Contexts
{
    public class BookManagementDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BookEFCore;Integrated Security=true;TrustServerCertificate=true;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(b => {
                b.Property(p => p.Name).IsRequired().HasMaxLength(200);
                b.Property(p => p.Description).HasMaxLength(200);
                b.Property(p => p.Title).HasMaxLength(100);

                b.HasOne(p => p.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(p => p.AuthorID);
            });

            modelBuilder.Entity<Author>(a => {
                a.Property(p => p.Name).IsRequired().HasMaxLength(200);

                a.Property(p => p.Address).HasMaxLength(200);
            });
        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!; 
    }
}
