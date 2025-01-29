

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface ICryptographyService
{
    public string GenerateSalt(int size = 32);
    public string HashPassword(string password, string salt, int iterations = 10000, int hashSize = 32);
}
