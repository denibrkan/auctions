using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            Console.WriteLine("--> Consuming auction deleted: " + context.Message.Id);

            await DB.DeleteAsync<Auction>(context.Message.Id);
        }
    }
}
