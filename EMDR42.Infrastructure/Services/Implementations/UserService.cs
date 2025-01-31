﻿using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Npgsql;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Implementations;

public class UserService : IUserService
{
    private readonly QueryFactory _query;
    private readonly ICryptographyService _cryptographyService;
    private readonly string TableName = "Users";

    public UserService(IDbConnectionManager connectionManager, ICryptographyService cryptographyService)
    {
        _query = connectionManager.PostgresQueryFactory;
        _cryptographyService = cryptographyService;
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

    public async Task CreatedUserAsync(UserModel model, NpgsqlTransaction transaction, QueryFactory query)
    {
        string salt = _cryptographyService.GenerateSalt();
        string hashedPassword = _cryptographyService.HashPassword(model.Password, salt);
        model.Password = hashedPassword;
        model.Salt = salt;

        var q = query.Query(TableName).AsInsert(model);

        await _query.ExecuteAsync(q, transaction);
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
            "Salt",
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
            .Select("Password",
            "Salt");

        if(query == null) return false;
        var result = await _query.FirstOrDefaultAsync<CheckPasswordModel>(query);

        string hash = _cryptographyService.HashPassword(request.Password, result.Salt);

        if (hash == result.Password) return true;
        else return false;
    }

    public async Task<int> UserConfirmAsync(string login)
    {
        var query = _query.Query(TableName).Where("Email", login).AsUpdate(new
        {
            IsConfirmed = true
        });

        return await _query.ExecuteAsync(query);
    }
}
