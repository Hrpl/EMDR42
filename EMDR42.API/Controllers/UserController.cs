using EMDR42.API.Services.Interfaces;
using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Templates;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Compilers;
using SqlKata.Execution;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMDR42.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly IUserProfileService _profileService;
    private readonly IContactService _contactService;
    private readonly IQualificationService _qualificationService;
    private readonly IDbConnectionManager _dbConnectionManager;

    public UserController(
        IEmailService emailService,
        IMapper mapper,
        IUserService userService,
        ILogger<UserController> logger,
        IUserProfileService profileService,
        IContactService contactService,
        IQualificationService qualificationService,
        IDbConnectionManager dbConnectionManager)
    {
        _emailService = emailService;
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
        _profileService = profileService;
        _contactService = contactService;
        _qualificationService = qualificationService;
        _dbConnectionManager = dbConnectionManager;
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> Get([FromQuery] string email)
    {
        var user = await _userService.CheckedUserByLoginAsync(email);
        if (user is true)
        {
            _logger.LogInformation("Пользователь прошёл проверку");
            await _userService.UserConfirmAsync(email);

            string htmlContent = @"
            <html>
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Redirecting...</title>
            </head>
            <body>
                <center>
                    <h1>Ваш email подтверждён</h1>
                    <h2>Теперь вы можете авторизоваться в приложении</h2>
                </center>
            </body>
            </html>";
            return Content(htmlContent, "text/html");
        }
        else return BadRequest("Неверный email адрес!");
    }

    // POST api/<UserController>
    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] LoginRequest req)
    {
        if (req.Email == "") return BadRequest("Поле Email не заполнено");
        _logger.LogError("Поле Email не заполнено");

        var findEmail = await _userService.CheckedUserByLoginAsync(req.Email);

        if (findEmail) return BadRequest("Пользователь с таким email уже существует");
        _logger.LogError("Пользователь с таким email уже существует");

        var user = _mapper.Map<UserModel>(req);
        using(var connection = _dbConnectionManager.PostgresDbConnection)
        {
            await connection.OpenAsync();

            using(var transaction = connection.BeginTransaction())
            {
                try
                {
                    var queryFactory = new QueryFactory(connection, new PostgresCompiler())
                    {
                        Logger = compiled => Console.WriteLine(compiled.ToString())
                    };

                    await _userService.CreatedUserAsync(user, transaction, queryFactory);
                    var id = await _userService.GetUserIdAsync(user.Email);

                    await _profileService.CreateUserProfileAsync(new UserProfileModel { UserId = id }, transaction, queryFactory);
                    await _contactService.CreateUserContactsAsync(new ContactsModel { UserId = id, ContactEmail = user.Email }, transaction, queryFactory);
                    await _qualificationService.CreateUserQualificationAsync(new QualificationModel { UserId = id }, transaction, queryFactory);

                    await transaction.CommitAsync();

                    var person = new SendEmailDto() { Email = req.Email, Name = "", Subject = "Confirm email", MessageBody = EmailTemplates.RegistrationEmailTemplate.Replace("@email", req.Email) };
                    _logger.LogInformation("Начало отправки сообщения пользователю");
                    await _emailService.SendEmail(person);

                    _logger.LogInformation("Сообщение отправлено пользователю");
                    return Ok(req.Email);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Ошибка при создании пользователя");
                    return BadRequest("Ошибка при создании пользователя: " + ex);
                }
            }
        }
    }
}
