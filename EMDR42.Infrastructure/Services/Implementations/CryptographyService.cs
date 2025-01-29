using EMDR42.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Implementations;

public class CryptographyService : ICryptographyService
{
    public string GenerateSalt(int size = 32)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var saltBytes = new byte[size];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }

    public string HashPassword(string password, string salt, int iterations = 10000, int hashSize = 32)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations, HashAlgorithmName.SHA256))
        {
            var hashBytes = pbkdf2.GetBytes(hashSize);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
