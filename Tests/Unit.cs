using Api.Extensions;
using System.Security.Claims;

namespace Tests
{
    public class Unit
    {
        public class ClaimsPrincipalExtensionsTests
        {
            [Fact]
            public void UserAuth0Id_Returns_NameIdentifier_WhenPresent()
            {
                var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "auth0|user-123"), new Claim("sub", "sub-value") };
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
                var result = user.UserAuth0Id();

                Assert.Equal("auth0|user-123", result);
            }
        }
    }
}