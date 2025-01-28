using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Implementations;
using EMDR42.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("api/qualification")]
[ApiController]
[Authorize]
public class QualificationController : ControllerBase
{
    private readonly IQualificationService _qualificationService;
    private readonly IJwtHelper _jwtHelper;
    private readonly IMapper _mapper;
    public QualificationController(IQualificationService qualificationService, IJwtHelper jwtHelper, IMapper mapper)
    {
        _qualificationService = qualificationService;
        _jwtHelper = jwtHelper;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Получение данных о квалификации пользователя. Необходим JWT")]
    public async Task<ActionResult<QualificationDTO>> Get()
    {
        try
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var id = await _jwtHelper.DecodJwt(token);

                var response = await _qualificationService.GetUserQualificationAsync(id);

                if (response != null) return Ok();
                else return BadRequest();
            }
            else return Unauthorized();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/<QualificationController>/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Обновление данных о квалификации пользователя. Необходим JWT")]
    public async Task<ActionResult> Put([FromBody] QualificationDTO request)
    {
        try
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var id = await _jwtHelper.DecodJwt(token);

                var model = _mapper.Map<QualificationModel>(request);
                model.UserId = id;

                await _qualificationService.UpdateUserQualificationAsync(model);
                return Ok();
            }
            else return Unauthorized();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
