using EMDR42.Domain.Commons.Filters;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Response;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMDR42.API.Controllers;

[Route("api/specialist")]
[ApiController]
public class FindSpecialistController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserProfileController> _logger;
    private readonly IFindSpecialistRepository _findSpecialistRepository;
    public FindSpecialistController(IMapper mapper, ILogger<UserProfileController> logger, IFindSpecialistRepository findSpecialistRepository)
    {
        _findSpecialistRepository = findSpecialistRepository ?? throw new ArgumentNullException(nameof(findSpecialistRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    /// <summary>
    /// Получение всех специалистов
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [SwaggerOperation(Summary = "Получение всех специалистов")]
    public async Task<ActionResult<IEnumerable<GetSpecialistsResponse>>> Get([FromQuery]GetAllSpecialist request)
    {
        try
        {
            var response = await _findSpecialistRepository.GetSpecialists(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching clients.");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }
}
