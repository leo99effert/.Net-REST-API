namespace Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _authrepo;
    public AuthController(IAuthRepository authrepo)
    {
      _authrepo = authrepo;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(RegisterDto request)
    {
      var response = await _authrepo.Register(new User { Username = request.Username }, request.Password);
      if (!response.Success) return BadRequest(response);
      return Ok(response);
    }
    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<int>>> Login(LoginDto request)
    {
      var response = await _authrepo.Login(request.Username, request.Password);
      if (!response.Success) return BadRequest(response);
      return Ok(response);
    }
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> Get()
    {
      return Ok(await _authrepo.GetUsers());
    }
    [HttpDelete]
    public async Task<ActionResult<ServiceResponse<int>>> Delete(int userId)
    {
      return Ok(await _authrepo.Delete(userId));
    }
  }
}