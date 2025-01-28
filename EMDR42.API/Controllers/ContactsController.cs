using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Implementations;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMDR42.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IJwtHelper _jwtHelper;
    private readonly IMapper _mapper;
    public ContactsController(IContactService contactService, IJwtHelper jwtHelper, IMapper mapper)
    {
        _contactService = contactService;
        _jwtHelper = jwtHelper;
        _mapper = mapper;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Получение контактов пользователя. Необходим JWT")]
    public async Task<ActionResult<ContactsDTO>> Get()
    {
        try
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var id = await _jwtHelper.DecodJwt(token);

                var response = await _contactService.GetUserContactsAsync(id);

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

    [HttpPut]
    [SwaggerOperation(Summary = "Обновление контактов пользователя. Необходим JWT")]
    public async Task<ActionResult> Put(ContactsDTO request)
    {
        try
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var id = await _jwtHelper.DecodJwt(token);

                var model = _mapper.Map<ContactsModel>(request);
                model.UserId = id;

                await _contactService.UpdateUserContactsAsync(model);
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
