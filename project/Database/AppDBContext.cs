    using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Database
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // work
            modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("Role")
            .HasValue<Author>("Author")
            .HasValue<Customer>("Customer");

            modelBuilder.Entity<Person>()
            .HasIndex(p => p.Email)
            .IsUnique();

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .Property(c => c.CashWallet)
                .HasDefaultValue(5000);
            modelBuilder.Entity<UserBook>()
            .HasKey(ub => new { ub.CustomerId, ub.BookId });

        }
    }
}
