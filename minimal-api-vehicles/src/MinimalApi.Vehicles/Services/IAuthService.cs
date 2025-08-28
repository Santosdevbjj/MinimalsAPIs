using MinimalApi.Vehicles.Models;

namespace MinimalApi.Vehicles.Services;

public interface IAuthService
{
    string GenerateToken(AdminUser user);
}
