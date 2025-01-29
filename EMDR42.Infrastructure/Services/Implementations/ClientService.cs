using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Implementations;

public class ClientService : IClientService
{
    private readonly QueryFactory _query;
    private const string TableName = "Clients";
    public ClientService(IDbConnectionManager connectionManager)
    {
        _query = connectionManager.PostgresQueryFactory;
    }

    public async Task CreateClientAsync(ClientModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        await _query.ExecuteAsync(query);
    }

    public async Task<GetAnyClientDTO> GetClientAsync(int clientId)
    {
        var query = _query.Query(TableName)
            .Where("Id", clientId)
            .Where("IsDeleted", false)
            .Select("Id",
            "UserName",
            "Country",
            "Language",
            "Email",
            "IsArchived");

        var result = await _query.FirstOrDefaultAsync<GetAnyClientDTO>(query);

        return result;
    }

    public async Task<int> DeleteClientAsync(int clientId)
    {
        var query = _query.Query(TableName)
            .Where("Id", clientId)
            .AsUpdate(new {IsDeleted = true});

        return await _query.ExecuteAsync(query);
    }

    public async Task<int> ArchiveClientAsync(int clientId, bool isArchived)
    {
        var query = _query.Query(TableName)
            .Where("Id", clientId)
            .When(isArchived, 
                q => q.AsUpdate(new { IsArchived = false }), 
                q => q.AsUpdate(new { IsArchived = true }));

        //если isArchived = true, значит пользователь архивирован и его надо разархивировать
        //если isArchived = false, пользователя надо архивировать

        return await _query.ExecuteAsync(query);
    }

    public async Task<int> UpdateClientAsync(int clientId, UpdateClientModel model)
    {
        var query = _query.Query(TableName)
            .Where("Id", clientId)
            .AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }

    //public async Task<IEnumerable<GetCleintDTO>> GetAllClientAsync(int userId)
    //{
    //    var query = _query.Query(TableName)
    //        .Where("UserId", userId)
    //        .Select("UserName",
    //        "Country",
    //        "Language",
    //        "Email");

    //    var result = await _query.GetAsync<GetCleintDTO>(query);

    //    return result;
    //}
}
