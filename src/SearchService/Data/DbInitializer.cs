using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.Services;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            await DB.InitAsync("SearchDb",
                    MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<Auction>()
                    .Key(x => x.Category, KeyType.Text)
                    .Key(x => x.Title, KeyType.Text)
                    .CreateAsync();

            using var scope = app.Services.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionHttpClient>();

            var auctions = await httpClient.GetAuctionsForSearchDbAsync();

            Console.WriteLine($"{auctions.Count} auctions returned from auctions service");

            if (auctions.Any())
                await DB.SaveAsync(auctions);
        }
    }
}
