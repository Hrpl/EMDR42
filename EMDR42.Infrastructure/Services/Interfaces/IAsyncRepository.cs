using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Response;
using EMDR42.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IAsyncRepository<TDTO, DModel> where TDTO : class where DModel : class
{
    public Task<TDTO> GetAsync(int id);
    public Task<int> CreateAsync(DModel model);
    public Task<int> UpdateAsync(DModel model);
    public Task DeleteAsync(int id);
}
