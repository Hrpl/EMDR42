using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface ISessionService
{
    public Task<int> CreateSessionAsync(SessionModel model);
    public Task<IEnumerable<SessionLogResponse>> GetSessionLogs(GetSessionLogs request, int userId);
}
