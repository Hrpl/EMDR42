using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Npgsql;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Implementations;

public class UserRepository : IUserRepository
{
    private readonly QueryFactory _query;
    private readonly ICryptographyService _cryptographyService;
    private readonly string TableName = "users";

    public UserRepository(IDbConnectionManager connectionManager, ICryptographyService cryptographyService)
    {
        _query = connectionManager.PostgresQueryFactory;
        _cryptographyService = cryptographyService;
    }

    /// <summary>
    /// Проверка наличия пользователя по email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task<bool> CheckedUserByLoginAsync(string login)
    {
        var query = _query.Query(TableName)
            .Where("email", login)
            .Select("email as Email");

        var result = await _query.FirstOrDefaultAsync<string>(query);

        if (result != null) return true;
        else return false;
    }
    /// <summary>
    /// Создание нового пользователя в системе
    /// </summary>
    /// <param name="model"></param>
    /// <param name="transaction"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    //todo:
    public async Task CreatedUserAsync(UserModel model, NpgsqlTransaction transaction, QueryFactory query)
    {
        string salt = _cryptographyService.GenerateSalt();
        string hashedPassword = _cryptographyService.HashPassword(model.Password, salt);
        model.Password = hashedPassword;
        model.Salt = salt;

        var q = query.Query(TableName).AsInsert(model);

        await _query.ExecuteAsync(q, transaction);
    }

    /// <summary>
    /// Удаление пользователя по email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task DeleteUserAsync(string login)
    {
        var query = _query.Query(TableName).Where("email", login).AsDelete();

        await _query.ExecuteAsync(query);
    }

    /// <summary>
    /// Получение данных о пользователи по его email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task<UserModel> GetUserAsync(string login)
    {
        var query = _query.Query(TableName)
            .Where("email", login)
            .Select("email as Email",
            "password as Password",
            "salt as Salt",
            "is_confirmed as IsConfirmed",
            "created_at as CreatedAt",
            "updated_at as UpdatedAt",
            "is_deleted as IsDeleted");

        var result = _query.FirstOrDefaultAsync<UserModel>(query);
        return result;
    }

    /// <summary>
    /// Получение id пользователя по его email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task<int> GetUserIdAsync(string login)
    {
        var query = _query.Query(TableName)
            .Where("email", login)
            .Select("id as Id");

        var result = await _query.FirstAsync<int>(query);
        return result;
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> LoginUserAsync(LoginRequest request)
    {
        var query = _query.Query(TableName)
            .Where("email", request.Email)
            .Select("password as Password",
            "salt as Salt");

        if(query == null) return false;
        var result = await _query.FirstOrDefaultAsync<CheckPasswordModel>(query);

        string hash = _cryptographyService.HashPassword(request.Password, result.Salt);

        if (hash == result.Password) return true;
        else return false;
    }

    /// <summary>
    /// Подтверждение электронной почты пользователя
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task<int> UserConfirmAsync(string login)
    {
        var query = _query.Query(TableName).Where("email", login).AsUpdate(new
        {
            is_confirmed = true
        });

        return await _query.ExecuteAsync(query);
    }
}
