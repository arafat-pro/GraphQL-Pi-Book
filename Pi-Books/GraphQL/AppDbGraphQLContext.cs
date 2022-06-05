using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pi_Books.Data.Models;

namespace Pi_Books.Data
{
    public class AppDbGraphQLContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbGraphQLContext(DbContextOptions<AppDbGraphQLContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Log> Logs { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}