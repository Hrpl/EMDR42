using EMDR42.Domain.Models;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IUserService
{
    public Task CreatedUserAsync(UserModel model);
    public Task<UserModel> GetUserAsync(string login);
    public Task<bool> CheckedUserByLoginAsync(string login);
    public Task<bool> LoginUserAsync(Domain.Commons.Request.LoginRequest request);
    public Task UserConfirmAsync(string login);
    public Task DeleteUserAsync(string login);
    public Task<int> GetUserIdAsync(string login);
}
