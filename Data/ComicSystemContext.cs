using ComicSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Data{
    public class ComicSystemContext : DbContext
{
    public ComicSystemContext(DbContextOptions<ComicSystemContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<ComicBook> ComicBooks { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<RentalDetail> RentalDetails { get; set; }
}

}