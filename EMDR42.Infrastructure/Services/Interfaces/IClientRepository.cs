using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Response;
using EMDR42.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IClientRepository
{
    /// <summary>
    /// Создание клиента
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task CreateClientAsync(ClientModel model);
    /// <summary>
    /// Получение списка пользователей с пагинацией, сортировкой и поиском
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<IEnumerable<ClientsResponse>> GetAllClientAsync(GetAllClientRequest request, int userId);
    /// <summary>
    /// Получение выбранного клиента
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    public Task<GetAnyClientDTO> GetClientAsync(int clientId);
    /// <summary>
    /// Удаление клиента по id
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    public Task<int> DeleteClientAsync(int clientId);
    /// <summary>
    /// Обновление клиента по id
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int> UpdateClientAsync(int clientId, UpdateClientModel model);
    /// <summary>
    /// Архивирование и разархивирование клиента. При isArchived = true, \n пользователя надо разархивировать, если isArchived = false - архивировать
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="isArchived"></param>
    /// <returns></returns>
    public Task<int> ArchiveClientAsync(int clientId, bool isArchived);
}
