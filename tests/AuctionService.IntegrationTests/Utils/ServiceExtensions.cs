using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests.Utils
{
    public static class ServiceExtensions
    {
        public static void RemoveDbContext(this IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);
        }

        public static void EnsureDbCreated(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AuctionDbContext>();

            dbContext.Database.Migrate();
            DbHelper.InitDbForTests(dbContext);
        }
    }
}
