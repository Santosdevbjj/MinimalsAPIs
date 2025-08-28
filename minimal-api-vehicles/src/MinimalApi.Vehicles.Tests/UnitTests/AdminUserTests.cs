using Xunit;

namespace MinimalApi.Vehicles.Tests.UnitTests;

public class AdminUserTests
{
    [Fact]
    public void BCrypt_Hash_And_Verify_Works()
    {
        var password = "My$ecureP@ss";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
    }
}
