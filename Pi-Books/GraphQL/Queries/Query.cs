using System.Linq;
using HotChocolate;
using Pi_Books.Data;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Queries
{
    public class Query
    {
        public IQueryable<Book> GetBook([Service] AppDbContext context)
        {
            return context.Books;
        }
    }
}