using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Implementations;

public class TherapyRepository(IDbConnectionManager connectionManager) : ITherapyRepository
{
    private readonly QueryFactory _query = connectionManager.PostgresQueryFactory;
    private const string TableName = "therapies";

    /// <summary>
    /// Создание записи о методах лечения
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> CreateAsync(TherapyModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        return await _query.ExecuteAsync(query);
    }
    /// <summary>
    /// Удаление записи о методах лечении
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        var query = _query.Query(TableName).Where("user_id", id).AsDelete();

        await _query.ExecuteAsync(query);
    }

    /// <summary>
    /// Получение данных о методах лечении
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TherapyDTO> GetAsync(int id)
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

    //// <summary>
    /// Обновление данных о методах лечении
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(TherapyModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }
}
