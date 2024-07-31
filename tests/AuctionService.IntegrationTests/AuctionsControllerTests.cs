using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    [Collection("Shared collection")]
    public class AuctionsControllerTests : IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private const string _validAuctionGuid = "afbee524-5972-4075-8800-7d1f9d7b0a0c";
        private const string _validCategoryGuid = "67c7bbce-bfa4-4195-b44e-da2fc3f7145e";

        public AuctionsControllerTests(CustomWebAppFactory customWebAppFactory)
        {
            _factory = customWebAppFactory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAllAuctions_ShouldReturn4Auctions()
        {
            var result = await _httpClient.GetFromJsonAsync<List<AuctionDTO>>("api/auctions");

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
        {
            var result = await _httpClient.GetFromJsonAsync<AuctionDTO>($"api/auctions/{_validAuctionGuid}");

            Assert.IsType<AuctionDTO>(result);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidGuid_Returns400()
        {
            var result = await _httpClient.GetAsync($"api/auctions/notaguid");

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateAuction_WithNoAuth_Returns401()
        {
            var auction = new CreateAuctionDTO { Title = "test auth auction" };

            var result = await _httpClient.PostAsJsonAsync("api/auctions", auction);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task CreateAuction_WithAuth_Returns201()
        {
            // arrange
            var auction = GetTestCreateAuction();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var result = await _httpClient.PostAsJsonAsync("api/auctions", auction);
            var createdAuction = await result.Content.ReadFromJsonAsync<AuctionDTO>();

            // assert
            result.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("bob", createdAuction.Seller);
        }

        [Fact]
        public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
        {
            // arrange
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            var auction = GetTestCreateAuction();
            auction.Title = null;

            // act
            var result = await _httpClient.PostAsJsonAsync("api/auctions", auction);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
        {
            // arrange
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            var auction = new UpdateAuctionDTO
            {
                Title = "Porsche 996 - bob"
            };

            // act
            var result = await _httpClient.PutAsJsonAsync($"api/auctions/{_validAuctionGuid}", auction);

            // assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
        {
            // arrange
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("deni"));

            var auction = new UpdateAuctionDTO
            {
                Title = "Porsche 996 - bob"
            };

            // act
            var result = await _httpClient.PutAsJsonAsync($"api/auctions/{_validAuctionGuid}", auction);

            // assert
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

            DbHelper.ReinitDbForTests(db);

            return Task.CompletedTask;
        }

        private CreateAuctionDTO GetTestCreateAuction()
        {
            return new CreateAuctionDTO
            {
                Title = "test",
                Description = "test",
                ImageUrl = "image.png",
                CategoryId = Guid.Parse(_validCategoryGuid),
                DateStart = DateTime.UtcNow,
                DateEnd = DateTime.UtcNow.AddDays(7),
                ReservePrice = 40
            };
        }
    }
}
