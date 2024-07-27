namespace AuctionService.DTOs
{
    public class BaseDTO
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
