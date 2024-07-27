using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<Auction>>> SearchAuctions([FromQuery] AuctionSearchObject searchObject)
        {
            var query = DB.PagedSearch<Auction, Auction>();

            query.Sort(a => a.Category, Order.Ascending);

            if (!string.IsNullOrEmpty(searchObject.SearchTerm))
                query.Match(Search.Full, searchObject.SearchTerm).SortByTextScore();

            query = searchObject.OrderBy switch
            {
                "newest" => query.Sort(x => x.DateCreated, Order.Descending),
                _ => query.Sort(x => x.DateEnd, Order.Ascending)
            };

            query = searchObject.FilterBy switch
            {
                "finished" => query.Match(x => x.DateEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(x => x.DateEnd < DateTime.UtcNow.AddHours(6) &&
                                                x.DateEnd > DateTime.UtcNow),
                _ => query.Match(x => x.DateEnd > DateTime.UtcNow)
            };

            query.PageNumber(searchObject.PageNumber);
            query.PageSize(searchObject.PageSize);

            var result = await query.ExecuteAsync();

            return Ok(new PagedResult<Auction>(result));
        }
    }
}
