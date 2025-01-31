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

public interface IQualificationRepository
{
    /// <summary>
    /// Получение квалификации пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<QualificationDTO> GetUserQualificationAsync(int id);
    /// <summary>
    /// Создание записи о квалификации пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <param name="transaction"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<int> CreateUserQualificationAsync(QualificationModel model);
    /// <summary>
    /// Обновление данных квалификации пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int> UpdateUserQualificationAsync(QualificationModel model);
    /// <summary>
    /// Удаление записи о квалификации пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeleteUserQualificationAsync(int id);
}
