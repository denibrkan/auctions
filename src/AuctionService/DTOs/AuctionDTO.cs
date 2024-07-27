namespace AuctionService.DTOs;

public class AuctionDTO : BaseDTO
{
    public int ReservePrice { get; set; }
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public string Status { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; }
}