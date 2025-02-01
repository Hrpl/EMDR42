using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Models;
using Npgsql;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Создание нового пользователя в системе
    /// </summary>
    /// <param name="model"></param>
    /// <param name="transaction"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<int> CreatedUserAsync(UserModel model);
    /// <summary>
    /// Получение данных о пользователи по его email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task<UserModel> GetUserAsync(string login);
    /// <summary>
    /// Проверка наличия пользователя по email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task<bool> CheckedUserByLoginAsync(string login);
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<bool> LoginUserAsync(Domain.Commons.Request.LoginRequest request);
    /// <summary>
    /// Подтверждение электронной почты пользователя
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task<int> UserConfirmAsync(string login);
    /// <summary>
    /// Удаление пользователя по email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task DeleteUserAsync(string login);
    /// <summary>
    /// Получение id пользователя по его email
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task<int> GetUserIdAsync(string login);
    /// <summary>
    /// Получение соли по почте пользователя
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<string?> GetSaltByEmail(string email);
    /// <summary>
    /// Смена пароля пользователя
    /// </summary>
    /// <returns></returns>
    public Task<int> ChangePasswordAsync(ChangePasswordRequest request);
    /// <summary>
    /// Обновление адреса электронной почты
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newEmail"></param>
    /// <returns></returns>
    public Task<int> ChangeEmailAsync(int userId, string newEmail);
}
