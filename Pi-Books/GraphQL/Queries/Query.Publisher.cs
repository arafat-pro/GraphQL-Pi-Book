using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using Pi_Books.Data;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Queries
{
    public partial class Query
    {
        [UseDbContext(typeof(AppDbGraphQLContext))]
        [UseProjection]
        public IQueryable<Publisher> GetPublisher([ScopedService] AppDbGraphQLContext context)
        {
            return context.Publishers;
        }
    }
}