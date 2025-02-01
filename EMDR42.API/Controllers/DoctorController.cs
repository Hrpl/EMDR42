using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMDR42.API.Controllers;

[Route("api/doctor")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserProfileController> _logger;
    public DoctorController(IMapper mapper, ILogger<UserProfileController> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    // GET: api/<DoctorController>
    [HttpGet]
    public Task<ActionResult<IEnumerable<Random>>> Get()
    {
        throw new NotImplementedException();
    }

    // GET api/<DoctorController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<DoctorController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<DoctorController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }
}
