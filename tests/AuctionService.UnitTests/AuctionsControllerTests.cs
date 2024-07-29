using AuctionService.Controllers;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AuctionService.RequestHelpers;
using AuctionService.UnitTests.Utils;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuctionService.UnitTests
{
    public class AuctionsControllerTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepository;
        private readonly Mock<IItemCategoryRepository> _itemCategoryRepository;
        private readonly Mock<IPublishEndpoint> _publishEndpoint;
        private readonly Fixture _fixture;
        private readonly IMapper _mapper;
        private readonly AuctionsController _controller;

        public AuctionsControllerTests()
        {
            _auctionRepository = new Mock<IAuctionRepository>();
            _itemCategoryRepository = new Mock<IItemCategoryRepository>();
            _publishEndpoint = new Mock<IPublishEndpoint>();
            _fixture = new Fixture();

            var mockMapper = new MapperConfiguration(mc =>
            {
                mc.AddMaps(typeof(MappingProfiles).Assembly);
            }).CreateMapper().ConfigurationProvider;

            _mapper = new Mapper(mockMapper);
            _controller = new AuctionsController(
                _mapper, _publishEndpoint.Object, _auctionRepository.Object, _itemCategoryRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = Helpers.GetClaimsPrincipal()
                    }
                }
            };
        }

        [Fact]
        public async Task GetAllAuctions_WithNoParams_Returns10Auctions()
        {
            // arrange
            var auctions = _fixture.CreateMany<AuctionDTO>(10).ToList();
            _auctionRepository.Setup(repo => repo.GetAll(null)).ReturnsAsync(auctions);

            // act
            var result = await _controller.GetAllAuctions(null);

            // assert
            Assert.Equal(10, result.Value.Count);
            Assert.IsType<ActionResult<List<AuctionDTO>>>(result);
        }

        [Fact]
        public async Task GetAuctionById_WithValidGuid_ReturnsAuction()
        {
            // arrange
            var auction = _fixture.Build<Auction>().Without(p => p.Item).Create();
            _auctionRepository.Setup(repo => repo.GetById(auction.Id)).ReturnsAsync(auction);

            // act
            var result = await _controller.GetAuctionById(auction.Id);

            //assert
            Assert.Equal(auction.Seller, result.Value.Seller);
            Assert.IsType<ActionResult<AuctionDTO>>(result);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidGuid_ReturnsNotFound()
        {
            // arrange
            _auctionRepository.Setup(repo => repo.GetById(Guid.NewGuid())).ReturnsAsync(value: null);

            // act
            var result = await _controller.GetAuctionById(Guid.NewGuid());

            //assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateAuction_WithValidCreateAuctionDTO_ReturnsCreatedAt()
        {
            var auction = _fixture.Create<CreateAuctionDTO>();
            var itemCategory = _fixture.Create<ItemCategory>();

            _auctionRepository
                .Setup(repo => repo.Add(It.IsAny<Auction>()));

            _itemCategoryRepository
                .Setup(repo => repo.GetById(auction.CategoryId))
                .ReturnsAsync(itemCategory);

            _auctionRepository
                .Setup(repo => repo.SaveChanges())
                .ReturnsAsync(value: true);

            var result = await _controller.CreateAuction(auction);
            var createdResult = result.Result as CreatedAtActionResult;

            Console.WriteLine($"--> Create Auction Result: {createdResult}");

            Assert.NotNull(createdResult);
            Assert.Equal("GetAuctionById", createdResult.ActionName);
            Assert.IsType<AuctionDTO>(createdResult.Value);
        }

        [Fact]
        public async Task CreateAuction_DateEndBeforeDateStart_ReturnsBadRequest()
        {
            var auction = _fixture.Build<CreateAuctionDTO>()
                                  .With(a => a.DateStart, DateTime.Now.AddDays(2)) // Ensure DateStart is in the future
                                  .With(a => a.DateEnd, DateTime.Now.AddDays(1)) // Ensure DateEnd is before DateStart
                                  .Create();

            var result = await _controller.CreateAuction(auction);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateAuction_FailedSave_Returns400BadRequest()
        {
            var auction = _fixture.Create<CreateAuctionDTO>();
            var itemCategory = _fixture.Create<ItemCategory>();

            _auctionRepository
                .Setup(repo => repo.SaveChanges())
                .ReturnsAsync(false);

            _itemCategoryRepository
                .Setup(repo => repo.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(itemCategory);

            var result = await _controller.CreateAuction(auction);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateAuction_WithUpdateAuctionDto_ReturnsOkResponse()
        {
            var updateAuctionDTO = _fixture.Create<UpdateAuctionDTO>();

            var auction = _fixture
                .Build<Auction>()
                .With(a => a.Seller, "test")
                .Without(p => p.Item)
                .Create();

            var item = _fixture
                .Build<Item>()
                .Without(i => i.Auction)
                .Create();

            auction.Item = item;

            _auctionRepository
                .Setup(repo => repo.GetById(auction.Id))
                .ReturnsAsync(auction);

            _auctionRepository
                .Setup(repo => repo.SaveChanges())
                .ReturnsAsync(true);

            var result = await _controller.UpdateAuction(auction.Id, updateAuctionDTO);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateAuction_WithInvalidUser_Returns403Forbid()
        {
            var updateAuctionDTO = _fixture.Create<UpdateAuctionDTO>();

            var auction = _fixture
                .Build<Auction>()
                .With(a => a.Seller, "invalid user")
                .Without(p => p.Item)
                .Create();

            _auctionRepository
                .Setup(repo => repo.GetById(auction.Id))
                .ReturnsAsync(auction);

            var result = await _controller.UpdateAuction(auction.Id, updateAuctionDTO);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task UpdateAuction_WithInvalidGuid_ReturnsNotFound()
        {
            var updateAuctionDTO = _fixture.Create<UpdateAuctionDTO>();

            _auctionRepository
                .Setup(repo => repo.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(value: null);

            var result = await _controller.UpdateAuction(Guid.NewGuid(), updateAuctionDTO);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAuction_WithValidUser_ReturnsOkResponse()
        {
            var auction = _fixture
                .Build<Auction>()
                .With(a => a.Seller, "test")
                .Without(p => p.Item)
                .Create();

            _auctionRepository
                .Setup(repo => repo.GetById(auction.Id))
                .ReturnsAsync(auction);

            _auctionRepository
                .Setup(repo => repo.SaveChanges())
                .ReturnsAsync(true);

            var result = await _controller.DeleteAuction(auction.Id);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAuction_WithInvalidGuid_Returns404Response()
        {
            var auction = _fixture
               .Build<Auction>()
               .With(a => a.Seller, "test")
               .Without(p => p.Item)
               .Create();

            _auctionRepository
                .Setup(repo => repo.GetById(auction.Id))
                .ReturnsAsync(value: null);

            var result = await _controller.DeleteAuction(auction.Id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAuction_WithInvalidUser_Returns403Response()
        {
            var auction = _fixture
               .Build<Auction>()
               .With(a => a.Seller, "invalid user")
               .Without(p => p.Item)
               .Create();

            _auctionRepository
                .Setup(repo => repo.GetById(auction.Id))
                .ReturnsAsync(auction);

            var result = await _controller.DeleteAuction(auction.Id);

            Assert.IsType<ForbidResult>(result);
        }
    }
}
