using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDTO>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Item.Category.Name))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Item.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Item.Description))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Item.ImageUrl));

        CreateMap<CreateAuctionDTO, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDTO, Item>();

        CreateMap<AuctionDTO, AuctionCreated>();
    }
}
