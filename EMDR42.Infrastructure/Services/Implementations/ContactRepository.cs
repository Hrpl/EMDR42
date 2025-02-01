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

    
    /// <inheritdoc />
    public async Task<int> CreateUserContactsAsync(ContactsModel model)
    {
        var q = _query.Query(TableName)
            .AsInsert(model);

        return await _query.ExecuteAsync(q);
    }

    
    /// <inheritdoc />
    public async Task<ContactsDTO> GetUserContactsAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("user_id", id)
            .Select("phone_number as PhoneNumber",
            "contact_email as ContactEmail",
            "contact_web_site as ContactWebSite");

        var result = await _query.FirstOrDefaultAsync<ContactsDTO>(query);

        return result;
    }

    
    /// <inheritdoc />
    public async Task<int> UpdateUserContactsAsync(ContactsModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }

    
    /// <inheritdoc />
    public async Task DeleteUserContactsAsync(int id)
    {
        var query = _query.Query(TableName).Where("user_id", id).AsDelete();

        await _query.ExecuteAsync(query);
    }

    public async Task<int> ChangeEmailAsync(int userId, string newEmail)
    {
        var query = _query.Query(TableName)
            .Where("user_id", userId)
            .AsUpdate(new
            {
                contact_email = newEmail
            });

        return await _query.ExecuteAsync(query);
    }
}
