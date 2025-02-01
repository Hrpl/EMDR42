using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Implementations;

public class TherapyRepository : ITherapyRepository
{
    private readonly QueryFactory _query;
    private const string TableName = "therapies";
    public TherapyRepository(IDbConnectionManager connectionManager)
    {
        _query = connectionManager.PostgresQueryFactory;
    }

    /// <inheritdoc />
    public async Task<int> CreateUserQualificationAsync(TherapyModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        return await _query.ExecuteAsync(query);
    }

    /// <inheritdoc />
    public async Task DeleteUserQualificationAsync(int id)
    {
        var query = _query.Query(TableName).Where("user_id", id).AsDelete();

        await _query.ExecuteAsync(query);
    }

    /// <inheritdoc />
    public async Task<TherapyDTO> GetUserTherapyAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("user_id", id)
            .Select("methods as Methods",
            "age_patients as AgePatients",
            "category_patients as CategoryPatients",
            "location as Location",
            "variant_sessions as VariantSessions",
            "problems as Problems");

        var result = await _query.FirstOrDefaultAsync<TherapyDTO>(query);

        return result;
    }

    /// <inheritdoc />
    public async Task<int> UpdateUserQualificationAsync(TherapyModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }
}
