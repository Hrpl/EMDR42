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

public interface IContactRepository : IAsyncRepository<ContactsDTO, ContactsModel>
{
    /// <summary>
    /// Обновление контактного адреса почты
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newEmail"></param>
    /// <returns></returns>
    public Task<int> ChangeEmailAsync(int userId, string newEmail);
}
