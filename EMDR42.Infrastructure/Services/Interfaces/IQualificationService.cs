using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Services.Interfaces;

public interface IQualificationService
{
    public Task<QualificationDTO> GetUserQualificationAsync(int id);
    public Task CreateUserQualificationAsync(QualificationModel model);
    public Task UpdateUserQualificationAsync(QualificationModel model);
}
