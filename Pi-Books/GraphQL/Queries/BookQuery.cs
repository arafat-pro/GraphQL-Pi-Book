using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using Pi_Books.Data;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Queries
{
    public class BookQuery
    {
        [UseDbContext(typeof(AppDbGraphQLContext))]
        public IQueryable<Book> GetBook([ScopedService] AppDbGraphQLContext context)
        {
            return context.Books;
        }
    }
}