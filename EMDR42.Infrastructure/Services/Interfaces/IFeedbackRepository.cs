using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Filters;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Models;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IFeedbackRepository : IAsyncRepository<FeedbackDTO, FeedbackModel>
{
    /// <summary>
    /// Если request.Feedback = false - получение обратной связи, если request.Feedback = true, получение отзывов 
    /// Если request.SortOnDate = 0, сначала новые отзывы, иначе сначала старые отзывы 
    /// Если request.SortBy = 0, поиск выполняется по имени, если request.SortBy = 1 - по ключевому слову в отзыве
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<IEnumerable<FeedbackDTO>> GetFeedbacksAsync(GetAllFeedbacksRequest request);
    /// <summary>
    /// Подтверждение обратной связи и перевод её в отзывы
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<int> ApproveFeedback(int id);
    /// <summary>
    /// Проверка, оставлял ли пользователь отзыв
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<bool> IsAlreadyFeedback(string email);
}
