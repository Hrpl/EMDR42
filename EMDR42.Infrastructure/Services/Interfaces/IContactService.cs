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

public interface IContactService
{
    public Task<ContactsDTO> GetUserContactsAsync(int id);
    public Task CreateUserContactsAsync(ContactsModel model, NpgsqlTransaction transaction, QueryFactory query);
    public Task UpdateUserContactsAsync(ContactsModel model);
    public Task DeleteUserContactsAsync(int id);
}
