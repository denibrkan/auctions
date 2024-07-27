using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Services
{
    public class AuctionHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuctionHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Auction>> GetAuctionsForSearchDbAsync()
        {
            var lastUpdated = await DB.Find<Auction, string>()
                                    .Sort(a => a.DateUpdated, Order.Descending)
                                    .Project(a => a.DateUpdated.ToString())
                                    .ExecuteFirstAsync();

            return await _httpClient.GetFromJsonAsync<List<Auction>>(
                _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);
        }
    }
}
