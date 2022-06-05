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
        public async Task<AddPublisherPayload> AddPublisher(
            AddPublisherInput input, [ScopedService] AppDbGraphQLContext context)
        {
            var publisher = new Publisher
            {
                Name = input.Name
            };
            context.Publishers.Add(publisher);
            await context.SaveChangesAsync();

            return new AddPublisherPayload(publisher);
        }
    }
}