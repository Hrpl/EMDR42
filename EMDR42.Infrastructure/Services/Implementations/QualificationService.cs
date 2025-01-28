using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Implementations;

public class QualificationService : IQualificationService
{
    private readonly QueryFactory _query;
    private const string TableName = "Qualifications";
    public QualificationService(IDbConnectionManager dbConnection)
    {
        _query = dbConnection.PostgresQueryFactory;
    }

    public async Task CreateUserQualificationAsync(QualificationModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        await _query.ExecuteAsync(query);
    }

    public async Task<QualificationDTO> GetUserQualificationAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("UserId", id)
            .Select("School",
            "Supervisor",
            "InPractic");

        var result = await _query.FirstOrDefaultAsync<QualificationDTO>(query);

        return result;
    }

    public async Task UpdateUserQualificationAsync(QualificationModel model)
    {
        var query = _query.Query(TableName).Where("UserId", model.UserId).AsUpdate(model);

        await _query.ExecuteAsync(query);
    }
}
