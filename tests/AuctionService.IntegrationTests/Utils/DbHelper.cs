using AuctionService.Data;
using AuctionService.Entities;

namespace AuctionService.IntegrationTests.Utils
{
    public class DbHelper
    {
        public static void InitDbForTests(AuctionDbContext db)
        {
            var categories = GetItemCategoriesForTest();
            db.ItemCategories.AddRange(categories);
            db.SaveChanges();

            db.Auctions.AddRange(GetAuctionsForTest(categories));
            db.SaveChanges();
        }

        public static void ReinitDbForTests(AuctionDbContext db)
        {
            db.Auctions.RemoveRange(db.Auctions);
            db.ItemCategories.RemoveRange(db.ItemCategories);
            db.SaveChanges();
            InitDbForTests(db);
        }

        private static List<Auction> GetAuctionsForTest(List<ItemCategory> categories)
        {
            return new List<Auction>
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
            };
        }

        private static List<ItemCategory> GetItemCategoriesForTest()
        {
            return new List<ItemCategory>
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
            }
            };
        }
    }
}
