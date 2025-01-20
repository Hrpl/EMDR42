using EMDR42.Domain.Commons.Response;
namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IJwtHelper
{
    public JwtResponse CreateJwtAsync(int userId);
    public Task<int> DecodJwt(string accessToken);
}
