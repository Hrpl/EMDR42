using EMDR42.Domain.Commons.Filters;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Response;
using EMDR42.Infrastructure.Services.Interfaces;
using SqlKata.Execution;

namespace EMDR42.Infrastructure.Services.Implementations;

public class FindSpecialistRepository : IFindSpecialistRepository
{
    private readonly QueryFactory _query;
    public FindSpecialistRepository (IDbConnectionManager connectionManager)
    {
        _query = connectionManager.PostgresQueryFactory;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetSpecialistsResponse>> GetSpecialists(GetAllSpecialist request)
    {
        var query = _query.Query("user_profile")
            .Where("is_public", true)
            .Select("user_id as UserId",
            "photo as Photo",
            "name as Name",
            "surname as Surname",
            "about_me as AboutMe",
            "clinic_name as ClinicName")
            .When(!(string.IsNullOrEmpty(request.Search)), q => q.WhereRaw($"user_profile.address like '{request.Search}%' or user_profile.name like '{request.Search}%'"))
            .Limit(request.PageSize)
            .Offset(request.Skip); ;

        var response = await _query.GetAsync<GetSpecialistsResponse>(query);

        return response;
    }
}
