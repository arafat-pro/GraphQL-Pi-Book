using System.Linq;
using HotChocolate;
using Pi_Books.Data;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Types
{
    public partial class BookType
    {
        private class Resolver
        {
            public Publisher GetPublisher([Parent]Book book, [ScopedService] AppDbGraphQLContext context)
            {
                return context.Publishers.FirstOrDefault(p => p.Id == book.PublisherId);
            }
        }
    }
}