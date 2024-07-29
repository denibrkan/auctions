using System.Security.Claims;

namespace AuctionService.UnitTests.Utils
{
    public class Helpers
    {
        public static ClaimsPrincipal GetClaimsPrincipal()
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    [new Claim(ClaimTypes.Name, "test")]
                ));
        }
    }
}
