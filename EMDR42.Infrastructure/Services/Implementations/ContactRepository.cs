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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EMDR42.Infrastructure.Services.Implementations;

public class ContactRepository : IContactRepository
{
    private readonly QueryFactory _query;
    private const string TableName = "contacts";
    public ContactRepository(IDbConnectionManager dbConnectionManager)
    {
        _query = dbConnectionManager.PostgresQueryFactory;
    }
    public async Task CreateUserContactsAsync(ContactsModel model, NpgsqlTransaction transaction, QueryFactory query)
    {
        var q = query.Query(TableName)
            .AsInsert(model);

        await _query.ExecuteAsync(q, transaction);
    }

    public async Task<ContactsDTO> GetUserContactsAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("user_id", id)
            .Select("phone_number",
            "contact_email",
            "contact_web_site");

        var result = await _query.FirstOrDefaultAsync<ContactsDTO>(query);

        return result;
    }

    public async Task<int> UpdateUserContactsAsync(ContactsModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }

    public async Task DeleteUserContactsAsync(int id)
    {
        var query = _query.Query(TableName).Where("user_id", id).AsDelete();

        await _query.ExecuteAsync(query);
    }
}
