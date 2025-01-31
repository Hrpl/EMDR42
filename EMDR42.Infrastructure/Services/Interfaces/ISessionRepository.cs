using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface ISessionRepository
{
    /// <summary>
    /// Создание записи о сессии
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int> CreateSessionAsync(SessionModel model);
    /// <summary>
    /// Получение логов сессии
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<IEnumerable<SessionLogResponse>> GetSessionLogs(GetSessionLogs request);
}
