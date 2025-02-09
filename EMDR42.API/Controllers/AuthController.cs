using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Response;
using EMDR42.Infrastructure.Services.Implementations;
using EMDR42.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("user")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IUserRepository _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IJwtHelper jwtHelper,
        IMapper mapper,
        IUserRepository userService,
        ILogger<AuthController> logger)
    {
        _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger;
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Авторизация пользователя")]
    public async Task<ActionResult<JwtResponse>> Auth(LoginRequest req)
    {

        try
        {
            var user = await _userService.GetUserAsync(req.Email);
            if (user != null && user.IsConfirmed == false)
            {
                _logger.LogError("Ваш email не подтверждён. Проверьте сообщения на почте.");
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = "Ваш email не подтверждён. Проверьте сообщения на почте."
                });
            }

            var check = await _userService.LoginUserAsync(req);

            if (!check)
            {
                _logger.LogError("Неверный логин или пароль");
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = "Неверный логин или пароль"
                });
            }

            var id = await _userService.GetUserIdAsync(req.Email);
            var jwt = _jwtHelper.CreateJwtAsync(id);

            return Ok(jwt);
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
}
