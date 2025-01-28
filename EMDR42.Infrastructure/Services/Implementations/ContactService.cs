using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EMDR42.Infrastructure.Services.Implementations;

public class ContactService : IContactService
{
    private readonly QueryFactory _query;
    private const string TableName = "Contacts";
    public ContactService(IDbConnectionManager dbConnectionManager)
    {
        _query = dbConnectionManager.PostgresQueryFactory;
    }
    public async Task CreateUserContactsAsync(ContactsModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        await _query.ExecuteAsync(query);
    }

    public async Task<ContactsDTO> GetUserContactsAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("UserId", id)
            .Select("PhoneNumber",
            "ContactEmail",
            "ContactWebSite");

        var result = await _query.FirstOrDefaultAsync<ContactsDTO>(query);

        return result;
    }

    public async Task UpdateUserContactsAsync(ContactsModel model)
    {
        var query = _query.Query(TableName).Where("UserId", model.UserId).AsUpdate(model);

        await _query.ExecuteAsync(query);
    }
}
