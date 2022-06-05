using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Pi_Books.Data;
using Pi_Books.Data.Models;
using Pi_Books.GraphQL.Mutations.Inputs;
using Pi_Books.GraphQL.Mutations.Payloads;

namespace Pi_Books.GraphQL.Mutations
{
    public partial class Mutation
    {
        [UseDbContext(typeof(AppDbGraphQLContext))]
        public async Task<AddBookPayload> AddBookAsync(
            AddBookInput input, [ScopedService] AppDbGraphQLContext context)
        {
            var book = new Book
            {
                Title = input.Title,
                Genre = input.Genre,
                PublisherId = input.PublisherId
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            return new AddBookPayload(book);
        }
    }
}