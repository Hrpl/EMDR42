using EMDR42.Domain.Commons.Filters;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IFindSpecialistRepository
{
    /// <summary>
    /// Получение списка всех доступных специалистов
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<GetSpecialistsResponse>> GetSpecialists(GetAllSpecialist request);
}
