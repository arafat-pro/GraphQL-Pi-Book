using HotChocolate;
using HotChocolate.Types;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL
{
    public class Subscription
    {
        [Topic]
        [Subscribe]
        public Publisher OnPublisherAdded([EventMessage] Publisher publisher) => publisher;
    }
}