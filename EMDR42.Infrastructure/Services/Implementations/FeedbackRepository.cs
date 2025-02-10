using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Filters;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EMDR42.Infrastructure.Services.Implementations;

public class FeedbackRepository(IDbConnectionManager manager) : IFeedbackRepository
{
    private readonly QueryFactory _query = manager.PostgresQueryFactory;
    private const string TableName = "feedbacks";

    ///<inheritdoc/>
    public async Task<int> ApproveFeedback(int id)
    {
        var query = _query.Query(TableName)
            .Where("id", id)
            .AsUpdate(new {
                is_approved = true
            });

        return await _query.ExecuteAsync(query);
    }
    /// <summary>
    /// Создание обратной связи
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> CreateAsync(FeedbackModel model)
    {
        var query = _query.Query(TableName)
            .AsInsert(model);

        return await _query.ExecuteAsync(query);
    }
    /// <summary>
    /// Удаление обратной связи
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(int id)
    {
        var query = _query.Query(TableName).Where("id", id).AsDelete();

        return await _query.ExecuteAsync(query);
    }

    public async Task<FeedbackDTO> GetAsync(int id)
    {
        var query = _query.Query(TableName)
            .LeftJoin("user", "user.email", "feedbacks.email")
            .LeftJoin("user_profile", "user_profile.user_id", "user.id")
            .Where("id", id)
            .Select("feedbacks.name as Name", "feedbacks.feedback as Feedback", "feedbacks.email as Email", "user_profile.photo as Photo");

        var res = await _query.FirstOrDefaultAsync<FeedbackDTO>(query);

        return res;

    }

    ///<inheritdoc/>
    public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksAsync(GetAllFeedbacksRequest request)
    {
        var query = _query.Query("feedbacks")
            .LeftJoin("user", "user.email", "feedbacks.email")
            .LeftJoin("user_profile", "user_profile.user_id", "user.id")
            .Where("feedbacks.is_approved", request.Feedback)
            .When(!(string.IsNullOrEmpty(request.Search)) && request.SortBy == 0, q => q.WhereRaw($"feedbacks.name like '{request.Search}%'"))
            .When(!(string.IsNullOrEmpty(request.Search)) && request.SortBy == 1, q => q.WhereRaw($"feedbacks.feedback like '{request.Search}%'"))
            .When(request.SortOnDate == 0, q => q.OrderByDesc("feedbacks.created_at"), q => q.OrderBy("feedbacks.created_at"))
            .Select("feedbacks.name as Name", "feedbacks.feedback as Feedback", "feedbacks.email as Email", "user_profile.photo as Photo")
            .Limit(request.PageSize)
            .Offset(request.Skip);

        var res = await _query.GetAsync<FeedbackDTO>(query);

        return res;
    }

    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(FeedbackModel model)
    {
        var query = _query.Query(TableName).Where("id", model.Email).AsUpdate(model);

        return await _query.ExecuteAsync(query);
    }
    ///<inheritdoc/>
    public async Task<bool> IsAlreadyFeedback(string email)
    {
        var query = _query.Query(TableName).Where("email", email).Select("id");

        var res = await _query.FirstOrDefaultAsync<int?>(query);

        if (res != null) return true;
        else return false;
    }
}
