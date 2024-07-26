namespace AuctionService.Entities;

public class Auction : BaseEntity
{
    public int ReservePrice { get; set; } = 0;
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighBid { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public Status Status { get; set; }
    public Item Item { get; set; }

    public bool HasReservePrice() => ReservePrice > 0;
}