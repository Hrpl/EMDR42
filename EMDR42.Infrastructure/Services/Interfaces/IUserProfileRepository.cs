using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using Npgsql;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IUserProfileRepository
{
    /// <summary>
    /// Получение данных профился пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<GetUserProfileDTO> GetUserProfilesAsync(int id);
    /// <summary>
    /// Создание записи профиля пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <param name="transaction"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<int> CreateUserProfileAsync(UserProfileModel model);
    /// <summary>
    /// Обновление данных профиля пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int> UpdateUserProfileAsync(UserProfileModel model);
    /// <summary>
    /// Удаление записи профиля пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeleteUserProfileAsync(int id);
    /// <summary>
    /// Обновление данных "О себе" в профиле
    /// </summary>
    /// <param name="aboutMe"></param>
    /// <returns></returns>
    public Task<int> UpdateAboutMeAsync(string aboutMe, int userId);
    /// <summary>
    /// Обновление фотографии пользователя
    /// </summary>
    /// <param name="photo"></param>
    /// <returns></returns>
    public Task<int> UpdatePhotoAsync(string userPhoto, int userId);
}
