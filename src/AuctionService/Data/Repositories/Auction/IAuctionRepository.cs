using AuctionService.DTOs;
using AuctionService.Entities;

namespace AuctionService.Data
{
    public interface IAuctionRepository
    {
        Task<List<AuctionDTO>> GetAll(string date);
        Task<Auction> GetById(Guid id);
        void Add(Auction auction);
        void Remove(Auction auction);
        Task<bool> SaveChanges();
    }
}
