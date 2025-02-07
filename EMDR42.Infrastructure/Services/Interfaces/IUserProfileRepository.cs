using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using Npgsql;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IUserProfileRepository : IAsyncRepository<GetUserProfileDTO, UserProfileModel>
{
    /// <summary>
    /// Обновление фотографии пользователя
    /// </summary>
    /// <param name="photo"></param>
    /// <returns></returns>
    public Task<int> UpdatePhotoAsync(string userPhoto, int userId);

    /// <summary>
    /// Обновление данных "О себе"
    /// </summary>
    /// <param name="aboutMe"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<int> UpdateAboutMeAsync(string aboutMe, int userId);
}
