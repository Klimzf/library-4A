using Microsoft.EntityFrameworkCore;
using HomeLibraryApp.Models;

namespace HomeLibraryApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .Property(b => b.ContentXML)
            .HasColumnType("xml"); 

        modelBuilder.Entity<Book>().ToTable("Books"); 
    }
}