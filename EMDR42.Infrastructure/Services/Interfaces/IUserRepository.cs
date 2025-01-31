using EMDR42.Domain.Models;
using Npgsql;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IUserRepository
{
    public Task CreatedUserAsync(UserModel model, NpgsqlTransaction transaction, QueryFactory query);
    public Task<UserModel> GetUserAsync(string login);
    public Task<bool> CheckedUserByLoginAsync(string login);
    public Task<bool> LoginUserAsync(Domain.Commons.Request.LoginRequest request);
    public Task<int> UserConfirmAsync(string login);
    public Task DeleteUserAsync(string login);
    public Task<int> GetUserIdAsync(string login);
}
