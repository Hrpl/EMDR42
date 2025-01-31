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
    public Task CreateClientAsync(ClientModel model);
    public Task<IEnumerable<ClientsResponse>> GetAllClientAsync(GetAllClientRequest request, int userId);
    public Task<GetAnyClientDTO> GetClientAsync(int clientId);
    public Task<int> DeleteClientAsync(int clientId);
    public Task<int> UpdateClientAsync(int clientId, UpdateClientModel model);
    public Task<int> ArchiveClientAsync(int clientId, bool isArchived);
}
