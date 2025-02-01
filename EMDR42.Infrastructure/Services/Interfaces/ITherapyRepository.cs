using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface ITherapyRepository
{
    /// <summary>
    /// Получение данных о методах лечении
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<TherapyDTO> GetUserTherapyAsync(int id);
    /// <summary>
    /// Создание записи о методах лечения
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int> CreateUserQualificationAsync(TherapyModel model);
    /// <summary>
    /// Обновление данных о методах лечении
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int> UpdateUserQualificationAsync(TherapyModel model);
    /// <summary>
    /// Удаление записи о методах лечении
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeleteUserQualificationAsync(int id);
}
