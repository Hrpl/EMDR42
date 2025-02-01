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

public class UserProfileRepository : IUserProfileRepository
{
    private readonly QueryFactory _query;
    private const string TableName = "user_profile";
    public UserProfileRepository(IDbConnectionManager connectionManager)
    {
        _query = connectionManager.PostgresQueryFactory;
    }

    /// <inheritdoc />
    public async Task<int> CreateUserProfileAsync(UserProfileModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        return await _query.ExecuteAsync(query);
    }

    /// <inheritdoc />
    public async Task<GetUserProfileDTO> GetUserProfilesAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("user_id", id)
            .Select("name as Name",
            "surname as Surname",
            "patronymic as Patronymic",
            "clinic_name as ClinicName",
            "about_me as AboutMe",
            "photo as Photo",
            "gender as Gender",
            "birthday as Birthday",
            "address as Address",
            "is_public as IsPublic");

        var result = await _query.FirstOrDefaultAsync<GetUserProfileDTO>(query);

        return result;
    }

    /// <inheritdoc />
    public async Task<int> UpdateUserProfileAsync(UserProfileModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }

    /// <inheritdoc />
    public async Task DeleteUserProfileAsync(int id)
    {
        var query = _query.Query(TableName).Where("user_id", id).AsDelete();

        await _query.ExecuteAsync(query);
    }

    /// <inheritdoc />
    public async Task<int> UpdateAboutMeAsync(string aboutMe, int userId)
    {
        var query = _query.Query(TableName)
            .Where("user_id", userId)
            .AsUpdate(new
            {
                about_me = aboutMe
            });

        return await _query.ExecuteAsync(query);
    }

    /// <inheritdoc />
    public async Task<int> UpdatePhotoAsync(string userPhoto, int userId)
    {
        var query = _query.Query(TableName)
            .Where("user_id", userId)
            .AsUpdate(new
            {
                photo = userPhoto
            });

        return await _query.ExecuteAsync(query);
    }
}
