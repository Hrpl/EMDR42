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

namespace EMDR42.Infrastructure.Services.Implementations;

public class ContactRepository(IDbConnectionManager dbConnectionManager) : IContactRepository
{
    private readonly QueryFactory _query = dbConnectionManager.PostgresQueryFactory;
    private const string TableName = "contacts";


    /// <summary>
    /// Создание части профиля с контактами
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> CreateAsync(ContactsModel model)
    {
        var q = _query.Query(TableName)
            .AsInsert(model);

        return await _query.ExecuteAsync(q);
    }

    /// <summary>
    /// Получение контактов пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ContactsDTO> GetAsync(int id)
    {
        var query = _query.Query(TableName)
            .Where("user_id", id)
            .Select("phone_number as PhoneNumber",
            "contact_email as ContactEmail",
            "contact_web_site as ContactWebSite");

        var result = await _query.FirstOrDefaultAsync<ContactsDTO>(query);

        return result;
    }


    /// <summary>
    /// Обновление контактов пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(ContactsModel model)
    {
        var query = _query.Query(TableName).Where("user_id", model.UserId).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }


    /// <summary>
    /// Удаление записи о контактах пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
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
