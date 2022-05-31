using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pi_Books.Data.Models;

namespace Pi_Books.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relation Between Book & BookAuthor
            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(bi => bi.BookId);

            //Relation Between Author & BookAuthor
            modelBuilder.Entity<BookAuthor>()
                .HasOne(a => a.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(ai => ai.AuthorId);

            modelBuilder.Entity<Log>().HasKey(pk => pk.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Log> Logs { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}