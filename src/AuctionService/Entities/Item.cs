namespace AuctionService.Entities;

public class Item : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public ItemCategory Category { get; set; }
    public Guid AuctionId { get; set; }
    public Auction Auction { get; set; }
}
