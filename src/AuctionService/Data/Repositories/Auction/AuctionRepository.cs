using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuctionRepository(AuctionDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AuctionDTO>> GetAll(string date)
        {
            var query = _dbContext.Auctions
                    .Include(a => a.Item.Category)
                    .OrderBy(a => a.Item.Category.Name)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(date))
                query = query.Where(x => x.DateUpdated.CompareTo(DateTime.Parse(date).AddSeconds(0.1).ToUniversalTime()) > 0);

            return await query.ProjectTo<AuctionDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public Task<Auction> GetById(Guid id)
        {
            return _dbContext.Auctions
                            .Include(a => a.Item.Category)
                            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public void Add(Auction auction)
        {
            _dbContext.Auctions.Add(auction);
        }

        public void Remove(Auction auction)
        {
            _dbContext.Remove(auction);
        }

        public async Task<bool> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
