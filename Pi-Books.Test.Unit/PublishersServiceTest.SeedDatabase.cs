using Pi_Books.Data.Models;

namespace Pi_Books.Tests.Unit
{
    public partial class PublishersServiceTest
    {
        public static void SeedDatabase()
        {
            var publishers = new List<Publisher>
            {
                    new Publisher() { Id=1, Name = "Manuals Lib" },
                    new Publisher() { Id=2, Name = "Open World" },
                    new Publisher() { Id=3, Name = "Maktabatul Furqan" },
                    new Publisher() { Id=4, Name = "Pothik Prokashon" },
                    new Publisher() { Id=5, Name = "Adarsha" },
                    new Publisher() { Id=6, Name = "Mujahid Prokashoni" }
            };
            appDbContext.Publishers.AddRange(publishers);

            var authors = new List<Author>()
            {
                new Author(){Id=1, Name="Author 1"},
                new Author(){Id=2, Name="Author 2"},
                new Author(){Id=3, Name="Author 3"}
            };
            appDbContext.Authors.AddRange(authors);

            var books = new List<Book>() {
            new Book(){
                Id=1,
                Title = "User Manual of Midea AC",
                Description = "A brief literature for end user to operate a Midea AC",
                IsRead = false,
                Genre = "Operation Manual",
                CoverUrl = "https://www.arafat.pro/...",
                DateAdded = DateTime.Now.AddDays(-10),
                PublisherId = 1 },
            new Book()
            {
                Id=2,
                Title = "Vivo Smart Phone Y1 User Guide",
                Description = "Complete handbook for a user to operate a Vivo Smart-phone",
                IsRead = false,
                Genre = "Handbook",
                //Author = "Vivo Ltd.",
                CoverUrl = "https://www.arafat.pro/...",
                DateAdded = DateTime.Now.AddDays(-10),
                PublisherId = 1
            }
            };
            appDbContext.Books.AddRange(books);

            var booksAuthors = new List<BookAuthor>() {
                new BookAuthor{Id=1, BookId=1, AuthorId=1},
                new BookAuthor{Id=2, BookId=1, AuthorId=2},
                new BookAuthor{Id=3, BookId=2, AuthorId=2}
            };
            appDbContext.BooksAuthors.AddRange(booksAuthors);

            appDbContext.SaveChanges();
        }
    }
}