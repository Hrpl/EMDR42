using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Npgsql;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Implementations;

public class UserProfileService : IUserProfileService
{
    private readonly QueryFactory _query;
    private const string TableName = "UserProfile";
    public UserProfileService(IDbConnectionManager connectionManager)
    {
        _query = connectionManager.PostgresQueryFactory;
    }
    public async Task CreateUserProfileAsync(UserProfileModel model, NpgsqlTransaction transaction, QueryFactory query)
    {
        var q = query.Query(TableName)
            .AsInsert(model);

        await _query.ExecuteAsync(q, transaction);
    }

    public async Task<GetUserProfileDTO> GetUserProfilesAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("UserId", id)
            .Select("Name",
            "Surname",
            "Patronymic",
            "Gender",
            "Birthday",
            "Address",
            "IsPublic");

        var result = await _query.FirstOrDefaultAsync<GetUserProfileDTO>(query);

        return result;
    }

    public async Task UpdateUserProfileAsync(UserProfileModel model)
    {
        var query = _query.Query(TableName).Where("UserId", model.UserId).AsUpdate(model);

        await _query.ExecuteAsync(query);
    }

    public async Task DeleteUserProfileAsync(int id)
    {
        var query = _query.Query(TableName).Where("UserId", id).AsDelete();

        await _query.ExecuteAsync(query);
    }
}
