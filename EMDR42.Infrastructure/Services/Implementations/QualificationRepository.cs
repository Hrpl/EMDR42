using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Implementations;

public class QualificationRepository(IDbConnectionManager dbConnection) : IQualificationRepository
{
    private readonly QueryFactory _query = dbConnection.PostgresQueryFactory;
    private const string TableName = "qualifications";

    /// <summary>
    /// Создание записи о квалификации пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> CreateAsync(QualificationModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        return await _query.ExecuteAsync(query);
    }


    /// <summary>
    /// Получение квалификации пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<QualificationDTO> GetAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("user_id", id)
            .Select("school as School",
            "supervisor as Supervisor",
            "in_practic as InPractic");

        var result = await _query.FirstOrDefaultAsync<QualificationDTO>(query);

        return result;
    }


    /// <summary>
    /// Обновление данных квалификации пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(QualificationModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }


    /// <summary>
    /// Удаление записи о квалификации пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        var query = _query.Query(TableName).Where("user_id", id).AsDelete();

        await _query.ExecuteAsync(query);
    }
}
