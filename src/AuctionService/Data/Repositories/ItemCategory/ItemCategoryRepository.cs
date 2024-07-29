
using AuctionService.Entities;

namespace AuctionService.Data
{
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly AuctionDbContext _dbContext;

        public ItemCategoryRepository(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ItemCategory> GetById(Guid id)
        {
            return await _dbContext.ItemCategories.FindAsync(id);
        }
    }
}
