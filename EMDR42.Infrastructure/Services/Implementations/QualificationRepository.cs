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

public class QualificationRepository : IQualificationRepository
{
    private readonly QueryFactory _query;
    private const string TableName = "qualifications";
    public QualificationRepository(IDbConnectionManager dbConnection)
    {
        _query = dbConnection.PostgresQueryFactory;
    }
    
    /// <inheritdoc />
    //todo:
    public async Task CreateUserQualificationAsync(QualificationModel model, NpgsqlTransaction transaction, QueryFactory query)
    {
        var q = query.Query(TableName)
            .AsInsert(model);

        await _query.ExecuteAsync(q, transaction);
    }

    
    /// <inheritdoc />
    public async Task<QualificationDTO> GetUserQualificationAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("user_id", id)
            .Select("school as School",
            "supervisor as Supervisor",
            "in_practic as InPractic");

        var result = await _query.FirstOrDefaultAsync<QualificationDTO>(query);

        return result;
    }

    
    /// <inheritdoc />
    public async Task<int> UpdateUserQualificationAsync(QualificationModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }

    
    /// <inheritdoc />
    public async Task DeleteUserQualificationAsync(int id)
    {
        var query = _query.Query(TableName).Where("user_id", id).AsDelete();

        await _query.ExecuteAsync(query);
    }
}
