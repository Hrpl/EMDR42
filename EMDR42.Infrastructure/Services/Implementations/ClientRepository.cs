using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Response;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Implementations;

public class ClientRepository : IClientRepository
{
    private readonly QueryFactory _query;
    private const string TableName = "сlients";
    public ClientRepository(IDbConnectionManager connectionManager)
    {
        _query = connectionManager.PostgresQueryFactory;
    }

    
    /// <inheritdoc />
    public async Task CreateClientAsync(ClientModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        await _query.ExecuteAsync(query);
    }

    
    /// <inheritdoc />
    public async Task<GetAnyClientDTO> GetClientAsync(int clientId)
    {
        var query = _query.Query(TableName)
            .Where("id", clientId)
            .Where("is_deleted", false)
            .Select("id as Id",
            "user_name as UserName",
            "country as Country",
            "language as Language",
            "email as Email",
            "is_archived as IsArchived");

        var result = await _query.FirstOrDefaultAsync<GetAnyClientDTO>(query);

        return result;
    }

    
    /// <inheritdoc />
    public async Task<int> DeleteClientAsync(int clientId)
    {
        var query = _query.Query(TableName)
            .Where("id", clientId)
            .AsUpdate(new {IsDeleted = true});

        return await _query.ExecuteAsync(query);
    }

    
    /// <inheritdoc />
    public async Task<int> ArchiveClientAsync(int clientId, bool isArchived)
    {
        var query = _query.Query(TableName)
            .Where("id", clientId)
            .When(isArchived, 
                q => q.AsUpdate(new { is_archived = false }), 
                q => q.AsUpdate(new { is_archived = true }));

        //если isArchived = true, значит пользователь архивирован и его надо разархивировать
        //если isArchived = false, пользователя надо архивировать

        return await _query.ExecuteAsync(query);
    }

    
    /// <inheritdoc />
    public async Task<int> UpdateClientAsync(int clientId, UpdateClientModel model)
    {
        var query = _query.Query(TableName)
            .Where("id", clientId)
            .AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }

    
    /// <inheritdoc />
    public async Task<IEnumerable<ClientsResponse>> GetAllClientAsync(GetAllClientRequest request, int userId)
    {
        var query = _query.Query("clients as c")
            .Join("sessions as s", "s.client_id", "c.id")
            .Where("c.user_id", userId)
            .When(!(request.IsArchived), q => q.Where("c.is_archived", true))
            .When(!(string.IsNullOrEmpty(request.Search)), q => q.WhereRaw($"c.user_name like '{request.Search}' or c.email like '{request.Search}'"))
            .Select("c.user_name as UserName",
            "c.country as Country",
            "c.email as Email")
            .SelectRaw("COUNT(s.client_id) as Sessions")
            .SelectRaw("(SELECT MAX(ss.created_at) FROM sessions as ss WHERE ss.client_id = c.id) as LastSession")
            .GroupBy("c.user_name", "c.country", "c.email", "c.id")
            .Limit(request.PageSize)
            .Offset(request.Skip);

        var result = await _query.GetAsync<ClientsResponse>(query);

        return result;
    }
}
