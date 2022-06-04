using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using Pi_Books.Data;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Types
{
    public partial class PublisherType : ObjectType<Publisher>
    {
        private class Resolvers
        {
            public IQueryable<Book> GetBooks([Parent]Publisher publisher, [ScopedService] AppDbGraphQLContext context)
            {
                return context.Books.Where(p => p.Id == publisher.Id);
            }
        }
    }
}