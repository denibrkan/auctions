using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);
            AuctionUpdated auction = context.Message;

            await DB.Update<Auction>()
                    .Match(a => a.ID == auction.Id)
                    .Modify(a => a.Title, auction.Title)
                    .Modify(a => a.Description, auction.Description)
                    .Modify(a => a.DateUpdated, auction.DateUpdated)
                    .ExecuteAsync();
        }
    }
}
