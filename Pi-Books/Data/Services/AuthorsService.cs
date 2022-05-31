using System.Linq;
using Pi_Books.Data.Models;
using Pi_Books.Data.ViewModels;

namespace Pi_Books.Data.Services
{
    public class AuthorsService
    {
        private AppDbContext _context;
        public AuthorsService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void AddAuthor(AuthorVM author)
        {
            var _author = new Author()
            {
                Name = author.FullName
            };
            _context.Authors.Add(_author);
            _context.SaveChanges();
        }

        public AuthorWithBooksVM GetAllBooksofAuthor(int authorId)
        {
            var _allBooksOfAuthor = _context.Authors.Where(a => a.Id == authorId).Select(ab => new AuthorWithBooksVM()
            {
                FullName = ab.Name,
                BookTitles = ab.BookAuthors.Select(b => b.Book.Title).ToList()
            }).FirstOrDefault();

            return _allBooksOfAuthor;
        }
    }
}