using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<AuctionDbContext>());
    }

    private static void SeedData(AuctionDbContext context)
    {
        context.Database.Migrate();

        if (context.Auctions.Any())
        {
            Console.WriteLine("Already have data - no need to seed");
            return;
        }

        var categories = new List<ItemCategory>
        {
            new ItemCategory
            {
                Id = Guid.Parse("67c7bbce-bfa4-4195-b44e-da2fc3f7145e"),
                Name = "Cars"
            },
            new ItemCategory
            {
                Id = Guid.Parse("7d7600e2-5808-4793-ae96-94675d2074f7"),
                Name = "Motorcycles"
            },
            new ItemCategory
            {
                Id = Guid.Parse("37f8f19c-3332-46d7-82cb-703908d71687"),
                Name = "Watches"
            },
            new ItemCategory
            {
                Id = Guid.Parse("2b82e65f-fba1-4985-b132-34d7306e751d"),
                Name = "Jewelry"
            },
            new ItemCategory
            {
                Id = Guid.Parse("52194ccf-1882-4c16-a1e1-5870b4540a59"),
                Name = "Musical Instruments"
            },
            new ItemCategory
            {
                Id = Guid.Parse("0080a2ad-ecc7-41c8-9e86-ba85eaea50f6"),
                Name = "Boats"
            },
            new ItemCategory
            {
                Id = Guid.Parse("2ec4a174-edfe-410f-8a89-d8959c04d533"),
                Name = "Travel"
            },
            new ItemCategory
            {
                Id = Guid.Parse("03124013-4afa-4e19-ae63-1ea541501e3a"),
                Name = "Real Estate"
            },
            new ItemCategory
            {
                Id = Guid.Parse("c67266a7-0ebd-4a35-a1ca-8fce08dd03bb"),
                Name = "Aviation"
            }
        };

        context.ItemCategories.AddRange(categories);
        context.SaveChangesAsync();

        var auctions = new List<Auction>()
        {
            // 1 Porsche 996
            new Auction
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Live,
                ReservePrice = 45000,
                Seller = "bob",
                DateStart = DateTime.UtcNow,
                DateEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Title = "Porsche 996",
                    Description = "A stunning Porsche 996 in excellent condition, featuring a powerful engine and luxurious interior.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2020/10/18/16/45/porsche-5665390_1280.jpg",
                    CategoryId = categories.First(c => c.Name == "Cars").Id
                }
            },
            // 2 Mercedes SL500
            new Auction
            {
                Id = Guid.Parse("1c776394-4cc1-4510-8096-a25da78cf447"),
                Status = Status.Live,
                ReservePrice = 100000,
                Seller = "lyla",
                DateStart = DateTime.UtcNow,
                DateEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Title = "Mercedes SL500",
                    Description = "A classic Mercedes SL500 convertible with low mileage and impeccable maintenance.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2018/09/20/14/23/mercedes-190-sl-3690985_1280.jpg",
                    CategoryId = categories.First(c => c.Name == "Cars").Id
                }
            },
            // 3 Rolex Submariner
            new Auction
            {
                Id = Guid.Parse("98996da8-8678-45e3-9525-727e7a2f6fc5"),
                Status = Status.Live,
                ReservePrice = 7500,
                Seller = "mark",
                DateStart = DateTime.UtcNow,
                DateEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Title = "Rolex Submariner 1975",
                    Description = "A Rolex Submariner luxury watch with a stainless steel bracelet and black dial.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/04/26/10/27/watch-1354042_960_720.jpg",
                    CategoryId = categories.First(c => c.Name == "Watches").Id
                }
            },
            // 4 Vintage Harley Davidson
            new Auction
            {
                Id = Guid.Parse("2b8af7d2-68dd-4dd1-9163-2855b524d022"),
                Status = Status.NotStarted,
                ReservePrice = 25000,
                Seller = "gogo",
                DateStart = DateTime.UtcNow.AddDays(1),
                DateEnd = DateTime.UtcNow.AddDays(7),
                Item = new Item
                {
                    Title = "Vintage Harley-Davidson",
                    Description = "A vintage Harley-Davidson motorcycle in pristine condition, perfect for collectors.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2019/07/13/10/44/motorcycle-4334549_640.jpg",
                    CategoryId = categories.First(c => c.Name == "Motorcycles").Id
                }
            },
            // 5 Piano
            new Auction
            {
                Id = Guid.Parse("f2815d3c-be08-474c-9538-cace99d68702"),
                Status = Status.Finished,
                ReservePrice = 6000,
                Seller = "selma",
                DateStart = DateTime.UtcNow.AddDays(-3),
                DateEnd = DateTime.UtcNow.AddDays(-1),
                Item = new Item
                {
                    Title = "Antique Grand Piano",
                    Description = "An antique grand piano with beautiful craftsmanship and a rich sound.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2015/01/16/12/10/piano-601386_640.jpg",
                    CategoryId = categories.First(c => c.Name == "Musical Instruments").Id
                }
            },

        };

        context.AddRange(auctions);

        context.SaveChanges();
    }
}
