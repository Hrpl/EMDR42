using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("feedback")]
[ApiController]
public class FeedbackController(ILogger<FeedbackController> logger, 
    IMapper mapper, 
    IFeedbackRepository feedbackRepository, 
    IUserRepository userRepository) : ControllerBase
{
    private readonly ILogger<FeedbackController> _logger = logger ;
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IFeedbackRepository _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    [HttpGet("all")]
    [Authorize]
    [SwaggerOperation(Summary = "Админ-панель. Получение обратной связи и отзывов. Необходим JWT",
        Description = "Если request.Feedback = false - получение обратной связи, если request.Feedback = true, получение отзывов \n " +
        "Если request.SortOnDate = 0, сначала новые отзывы, иначе сначала старые отзывы \n " +
        "Если request.SortBy = 0, поиск выполняется по имени, если request.SortBy = 1 - по ключевому слову в отзыве")]
    public async Task<ActionResult<IEnumerable<FeedbackDTO>>> Get(GetAllFeedbacksRequest request)
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "Invalid user ID in token."
                });
            }

            var isAdmin = await _userRepository.IsAdmin(Convert.ToInt32(userId));

            if (!isAdmin)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = "Forbidden",
                    Detail = "Invalid user ."
                });
            }

            var response = await _feedbackRepository.GetFeedbacksAsync(request);

            if (response == null)
            {
                _logger.LogError("При запросе данных произошла ошибка");
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = "При запросе данных произошла ошибка"
                });
            }
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

    
    [HttpPost("create")]
    public Task<ActionResult> Post([FromBody] FeedbackDTO request)
    {
    }

    
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
        
    }

    
    [HttpDelete("{id}")]
    [Authorize]
    [SwaggerOperation(Summary = "Удаление отзыва/обратной связи. Необходим JWT")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "Invalid user ID in token."
                });
            }

            var isAdmin = await _userRepository.IsAdmin(Convert.ToInt32(userId));

            if (!isAdmin)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = "Forbidden",
                    Detail = "Invalid user ."
                });
            }

            var result = await _feedbackRepository.DeleteAsync(id);
            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при удалении пользователя, возможно пользователь не найден: id = {id}");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при удалении пользователя, возможно пользователь не найден"
                });
            }

            return NoContent();
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
