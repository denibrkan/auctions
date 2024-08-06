using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public AuctionsController(IMapper mapper, IPublishEndpoint publishEndpoint, IAuctionRepository auctionRepository, IItemCategoryRepository itemCategoryRepository)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _auctionRepository = auctionRepository;
        _itemCategoryRepository = itemCategoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDTO>>> GetAllAuctions(string date)
    {
        return await _auctionRepository.GetAll(date);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDTO>> GetAuctionById(Guid id)
    {
        var auction = await _auctionRepository.GetById(id);

        if (auction == null) return NotFound();

        return _mapper.Map<AuctionDTO>(auction);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDTO auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);

        auction.Seller = User.Identity.Name;

        if (auction.DateStart > DateTime.UtcNow)
            auction.Status = Status.Upcoming;

        if (auction.DateEnd <= auction.DateStart)
            return BadRequest("Invalid start and end dates for the auction");

        _auctionRepository.Add(auction);

        var newAuction = _mapper.Map<AuctionDTO>(auction);
        var category = await _itemCategoryRepository.GetById(auction.Item.CategoryId);
        newAuction.Category = category.Name;

        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

        var result = await _auctionRepository.SaveChanges();

        if (!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetAuctionById),
            new { auction.Id }, newAuction);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDTO updateAuctionDto)
    {
        var auction = await _auctionRepository.GetById(id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name) return Forbid();

        auction.Item.Title = updateAuctionDto.Title;
        auction.Item.Description = updateAuctionDto.Description;
        auction.DateUpdated = DateTime.UtcNow;

        var auctionUpdated = new AuctionUpdated
        {
            Id = auction.Id.ToString(),
            Title = auction.Item.Title,
            Description = auction.Item.Description,
            DateUpdated = auction.DateUpdated
        };

        await _publishEndpoint.Publish(auctionUpdated);

        var result = await _auctionRepository.SaveChanges();

        if (result) return Ok();

        return BadRequest("Problem saving changes");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _auctionRepository.GetById(id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name) return Forbid();

        _auctionRepository.Remove(auction);

        await _publishEndpoint.Publish(new AuctionDeleted { Id = id.ToString() });

        var result = await _auctionRepository.SaveChanges();

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}
