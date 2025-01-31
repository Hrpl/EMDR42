﻿using EMDR42.Domain.Commons.DTO;
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
    public Task CreateUserProfileAsync(UserProfileModel model, NpgsqlTransaction transaction, QueryFactory query);
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
}
