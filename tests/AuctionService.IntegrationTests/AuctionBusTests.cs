using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Utils;
using Contracts;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    [Collection("Shared collection")]
    public class AuctionBusTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestHarness _testHarness;
        private const string _validCategoryGuid = "67c7bbce-bfa4-4195-b44e-da2fc3f7145e";

        public AuctionBusTests(CustomWebAppFactory customWebAppFactory)
        {
            _factory = customWebAppFactory;
            _httpClient = _factory.CreateClient();
            _testHarness = _factory.Services.GetTestHarness();
        }

        [Fact]
        public async Task CreateAuction_WithValidObject_ShouldPublishAuctionCreated()
        {
            // arrange
            var auction = GetTestCreateAuction();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var result = await _httpClient.PostAsJsonAsync("api/auctions", auction);

            // assert
            Assert.True(await _testHarness.Published.Any(message => message.MessageType == typeof(AuctionCreated)));
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
