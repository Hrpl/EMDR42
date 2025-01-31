using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Implementations;

public class SessionRepository : ISessionRepository
{
    private readonly QueryFactory _query;
    private readonly ILogger<SessionRepository> _logger;
    private const string TableName = "sessions";
    public SessionRepository(IDbConnectionManager dbConnection, ILogger<SessionRepository> logger)
    {
        _query = dbConnection.PostgresQueryFactory ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger;
    }

    /// <summary>
    /// Создание записи о сессии
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> CreateSessionAsync(SessionModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var query = _query.Query(TableName).AsInsert(model);
            return await _query.ExecuteAsync(query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании клиента.");
            throw; 
        }
    }
    /// <summary>
    /// Получение логов сессии
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IEnumerable<SessionLogResponse>> GetSessionLogs(GetSessionLogs request)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var query = _query.Query(TableName)
                .Where("created_at", ">", request.Start)
                .Where("created_at", "<", request.End)
                .Where("client_id", request.ClientId)
                .Select("created_at as CreatedAt",
                "duration as Duration");

            var result = await _query.GetAsync<SessionLogResponse>(query);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании клиента.");
            throw;
        }

    }
}
