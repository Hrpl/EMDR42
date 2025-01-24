using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Implementations;

public class UserService : IUserService
{
    private readonly QueryFactory _query;
    private readonly string TableName = "Users";
    public UserService(IDbConnectionManager connectionManager)
    {
        _query = connectionManager.PostgresQueryFactory;
    }

    public async Task<bool> CheckedUserByLoginAsync(string login)
    {
        var query = _query.Query(TableName)
            .Where("Email", login)
            .Select("Email");

        var result = await _query.FirstOrDefaultAsync<string>(query);

        if (result != null) return true;
        else return false;
    }

    public async Task CreatedUserAsync(UserModel model)
    {
        var query = _query.Query(TableName).AsInsert(model);

        await _query.ExecuteAsync(query);
    }

    public async Task DeleteUserAsync(string login)
    {
        var query = _query.Query(TableName).Where("Email", login).AsDelete();

        await _query.ExecuteAsync(query);
    }

    public Task<UserModel> GetUserAsync(string login)
    {
        var query = _query.Query(TableName)
            .Where("Email", login)
            .Select("Email",
            "Password",
            "IsConfirmed",
            "CreatedAt",
            "UpdatedAt",
            "IsDeleted");

        var result = _query.FirstOrDefaultAsync<UserModel>(query);
        return result;
    }

    public async Task<int> GetUserIdAsync(string login)
    {
        var query = _query.Query(TableName)
            .Where("Email", login)
            .Select("Id");

        var result = await _query.FirstAsync<int>(query);
        return result;
    }

    public async Task<bool> LoginUserAsync(LoginRequest request)
    {
        var query = _query.Query(TableName)
            .Where("Email", request.Email)
            .Where("Password", request.Password)
            .Select("Email");

        var result = await _query.FirstOrDefaultAsync<string>(query);

        if (result != null) return true;
        else return false;
    }

    public async Task UserConfirmAsync(string login)
    {
        var query = _query.Query(TableName).Where("Email", login).AsUpdate(new
        {
            IsConfirmed = true
        });

        await _query.ExecuteAsync(query);
    }
}
