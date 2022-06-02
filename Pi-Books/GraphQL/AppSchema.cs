using GraphQL.Types;
using Pi_Books.GraphQL.Queries;

namespace Pi_Books.GraphQL
{
    public class AppSchema:Schema
    {
        public AppSchema(BookQuery query)
        {
            this.Query = query;
        }
    }
}