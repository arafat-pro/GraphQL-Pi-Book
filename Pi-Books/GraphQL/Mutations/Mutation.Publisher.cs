using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using Pi_Books.Data;
using Pi_Books.Data.Models;
using Pi_Books.GraphQL.Mutations.Inputs;
using Pi_Books.GraphQL.Mutations.Payloads;

namespace Pi_Books.GraphQL.Mutations
{
    public partial class Mutation
    {
        [UseDbContext(typeof(AppDbGraphQLContext))]
        public async Task<AddPublisherPayload> AddPublisherAsync(
            AddPublisherInput input,
            [ScopedService] AppDbGraphQLContext context,
            [Service]ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var publisher = new Publisher
            {
                Name = input.Name
            };
            context.Publishers.Add(publisher);
            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(Subscription.OnPublisherAdded), publisher, cancellationToken);

            return new AddPublisherPayload(publisher);
        }
    }
}