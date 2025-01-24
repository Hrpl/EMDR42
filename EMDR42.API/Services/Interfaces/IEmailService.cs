using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Response;

namespace EMDR42.API.Services.Interfaces;

public interface IEmailService
{
    public Task<BaseResponseMessage> SendEmail(SendEmailDto data, CancellationToken ct = default);
}
