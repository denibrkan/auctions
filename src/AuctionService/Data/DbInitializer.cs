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
                Id = Guid.Parse("e1879b3c-ff9f-4dd0-9b1f-3daaf597b942"),
                Name = "Experiences"
            },
            new ItemCategory
            {
                Id = Guid.Parse("6e72b67a-5ae2-4b7d-b725-6d9fdb2dfd53"),
                Name = "Art"
            },
            new ItemCategory
            {
                Id = Guid.Parse("bc05ec19-7306-40df-bcd5-4e21edc4b354"),
                Name = "Sports Gear"
            },
            new ItemCategory
            {
                Id = Guid.Parse("c67266a7-0ebd-4a35-a1ca-8fce08dd03bb"),
                Name = "Other"
            }
        };

        context.ItemCategories.AddRange(categories);
        context.SaveChangesAsync();

        var auctions = new List<Auction>()
        {
            
            // 1 Mercedes SL500
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
            // 2 Scuba Diving Gear
            new Auction
            {
                Id = Guid.Parse("70adb03a-0abc-4ffb-9c1b-78aa72e10d3b"),
                Status = Status.Live,
                ReservePrice = 200,
                Seller = "pedro",
                DateStart = DateTime.UtcNow.AddDays(-1),
                DateEnd = DateTime.UtcNow.AddHours(8),
                Item = new Item
                {
                    Title = "Scuba Diving Gear",
                    Description = "Able to last 2 minutes under the water. Professional gear.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2015/11/18/16/38/diving-1049477_960_720.jpg",
                    CategoryId = categories.First(c => c.Name == "Other").Id
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
                Status = Status.Upcoming,
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
            // 6 Watch
            new Auction
            {
                Id = Guid.Parse("4e6a11f5-d0a8-4eb3-9e60-1a0d5e6e8a56"),
                Status = Status.Live,
                ReservePrice = 15000,
                Seller = "james",
                DateStart = DateTime.UtcNow.AddDays(-5),
                DateEnd = DateTime.UtcNow.AddDays(2),
                Item = new Item
                {
                    Title = "Luxury Swiss Watch",
                    Description = "A pristine luxury Swiss watch with precision timekeeping and elegant design.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2022/06/09/12/23/watch-7252465_1280.jpg",
                    CategoryId = categories.First(c => c.Name == "Watches").Id
                }
            },
            // 7 Car
            new Auction
            {
                Id = Guid.Parse("3a2d7a5e-8c7b-4a22-9a28-8fbc97ae24e8"),
                Status = Status.Upcoming,
                ReservePrice = 80000,
                Seller = "maria",
                DateStart = DateTime.UtcNow.AddDays(1),
                DateEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Title = "Classic Volskwagen",
                    Description = "A restored classic sports car with modern upgrades and high performance.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/03/27/19/44/car-1283947_640.jpg",
                    CategoryId = categories.First(c => c.Name == "Cars").Id
                }
            },
            // 8 Painting
            new Auction
            {
                Id = Guid.Parse("7d6e5f3b-bbb6-4fd5-88f0-caf2e3836d7e"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "linda",
                DateStart = DateTime.UtcNow.AddDays(-2),
                DateEnd = DateTime.UtcNow.AddDays(5),
                Item = new Item
                {
                    Title = "Vintage Oil Painting",
                    Description = "A vintage oil painting from a renowned artist, capturing a serene landscape.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2015/03/30/11/01/paintings-698290_1280.jpg",
                    CategoryId = categories.First(c => c.Name == "Art").Id
                }
            },
            // 9 Signed Football Shirt
            new Auction
            {
                Id = Guid.Parse("1c7f7bbd-2e88-4d5e-9071-5a3c81b176bc"),
                Status = Status.Upcoming,
                ReservePrice = 120,
                Seller = "alex",
                DateStart = DateTime.UtcNow.AddDays(2),
                DateEnd = DateTime.UtcNow.AddDays(12),
                Item = new Item
                {
                    Title = "Signed Football Shirt",
                    Description = "",
                    ImageUrl = "https://cdn.pixabay.com/photo/2020/08/02/09/25/jersey-5457156_1280.jpg",
                    CategoryId = categories.First(c => c.Name == "Sports Gear").Id
                }
            },
            // 10 Experience
            new Auction
            {
                Id = Guid.Parse("5a4e9a76-d8d6-4f12-b24d-2e3f2d3b45b7"),
                Status = Status.Finished,
                ReservePrice = 5000,
                Seller = "john",
                SoldAmount = 23000,
                Winner = "isak",
                DateStart = DateTime.UtcNow.AddDays(-10),
                DateEnd = DateTime.UtcNow.AddDays(-5),
                Item = new Item
                {
                    Title = "Luxury Yacht Cruise",
                    Description = "A week-long luxury yacht cruise through the Mediterranean Sea.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2022/01/31/23/35/sunset-6985210_1280.jpg",
                    CategoryId = categories.First(c => c.Name == "Experiences").Id
                }
            }
        };

        context.AddRange(auctions);

        context.SaveChanges();
    }
}
