using EMDR42.Domain.Models;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IUserProfileService
{
    public Task<GetUserProfileModel> GetUserProfilesAsync(int id);
    public Task CreateUserProfileAsync(UserProfileModel model);
    public Task UpdateUserProfileAsync(UserProfileModel model);
}
