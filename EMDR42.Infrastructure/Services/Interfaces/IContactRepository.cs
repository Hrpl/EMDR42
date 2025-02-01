using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using Npgsql;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IContactRepository
{
    /// <summary>
    /// Получение контактов пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ContactsDTO> GetUserContactsAsync(int id);
    /// <summary>
    /// Создание части профиля с контактами
    /// </summary>
    /// <param name="model"></param>
    /// <param name="transaction"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<int> CreateUserContactsAsync(ContactsModel model);
    /// <summary>
    /// Обновление контактов пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int> UpdateUserContactsAsync(ContactsModel model);
    /// <summary>
    /// Удаление записи о контактах пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeleteUserContactsAsync(int id);
    /// <summary>
    /// Обновление контактного адреса почты
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newEmail"></param>
    /// <returns></returns>
    public Task<int> ChangeEmailAsync(int userId, string newEmail);
}
