using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using Npgsql;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IUserProfileService
{
    public Task<GetUserProfileDTO> GetUserProfilesAsync(int id);
    public Task CreateUserProfileAsync(UserProfileModel model, NpgsqlTransaction transaction, QueryFactory query);
    public Task<int> UpdateUserProfileAsync(UserProfileModel model);
    public Task DeleteUserProfileAsync(int id);
}
