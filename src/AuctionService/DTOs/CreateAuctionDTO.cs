using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class CreateAuctionDTO
{

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public int ReservePrice { get; set; }

    [Required]
    public DateTime DateStart { get; set; }

    [Required]
    public DateTime DateEnd { get; set; }

}