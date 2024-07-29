using AuctionService.Entities;

namespace AuctionService.Data
{
    public interface IItemCategoryRepository
    {
        Task<ItemCategory> GetById(Guid id);
    }
}
