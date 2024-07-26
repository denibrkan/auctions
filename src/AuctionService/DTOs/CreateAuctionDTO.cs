using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class CreateAuctionDto
{

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string CategoryId { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public int ReservePrice { get; set; }

    [Required]
    public DateTime DateStart { get; set; }

    [Required]
    public DateTime DateEnd { get; set; }

}