using System;
using System.Collections.Generic;
using System.Linq;
using Pi_Books.Data.Models;
using Pi_Books.Data.ViewModels;

namespace Pi_Books.Data.Services
{
    public class BooksService
    {
        private AppDbContext _context;
        public BooksService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public void AddBook(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                //Author = book.Author,
                CoverUrl = book.CoverUrl,
                Genre = book.Genre,
                IsRead = book.IsRead,
                Rating = book.IsRead ? book.Rating.Value : null,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                DateAdded = DateTime.Now
            };
            _context.Add(_book);
            _context.SaveChanges();

        }

        public void AddBookWithAuthors(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                //Author = book.Author,
                CoverUrl = book.CoverUrl,
                Genre = book.Genre,
                IsRead = book.IsRead,
                Rating = book.IsRead ? book.Rating.Value : null,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId,

            };
            _context.Add(_book);
            _context.SaveChanges();
            foreach (var authorId in book.AuthorIds)
            {
                var _bookAuthor = new BookAuthor()
                {
                    BookId = _book.Id,
                    AuthorId = authorId
                };
                _context.BooksAuthors.Add(_bookAuthor);
                _context.SaveChanges();
            }
        }

        public List<Book> GetAllBooks() => _context.Books.ToList();

        public BookWithAuthorsVM GetTheBook(int bookId)
        {
            var _bookWithAuthors = _context.Books
                .Where(b => b.Id == bookId)
                .Select(book => new BookWithAuthorsVM()
                {
                    Title = book.Title,
                    Description = book.Description,
                    CoverUrl = book.CoverUrl,
                    Genre = book.Genre,
                    IsRead = book.IsRead,
                    Rating = book.IsRead ? book.Rating.Value : null,
                    DateRead = book.IsRead ? book.DateRead.Value : null,
                    PublisherName = book.Publisher.Name,
                    AuthorNames = book.BookAuthors.Select(a => a.Author.Name).ToList()
                })
                .FirstOrDefault();

            return _bookWithAuthors;
        }

        public Book ModifyBook(int bookId, BookVM book)
        {
            var bookinDb = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookinDb != null)
            {
                bookinDb.Title = book.Title;
                bookinDb.Description = book.Description;
                //bookinDb.Author = book.Author;
                bookinDb.CoverUrl = book.CoverUrl;
                bookinDb.Genre = book.Genre;
                bookinDb.IsRead = book.IsRead;
                bookinDb.Rating = book.IsRead ? book.Rating.Value : null;
                bookinDb.DateRead = book.IsRead ? book.DateRead.Value : null;

                _context.SaveChanges();
            }
            return bookinDb;
        }

        public void DeleteTheBook(int bookId)
        {
            var bookinDb = _context.Books.FirstOrDefault(b => b.Id == bookId);

            if (bookinDb != null)
            {
                _context.Books.Remove(bookinDb);
                _context.SaveChanges();
            }
        }
    }
}