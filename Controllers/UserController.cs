namespace Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(RegisterDto request)
    {
      var response = await _userService.Register(new User { Username = request.Username }, request.Password);
      if (response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<int>>> Login(LoginDto request)
    {
      var response = await _userService.Login(request.Username, request.Password);
      if(response.Success) return Ok(response);
      return BadRequest(response);      
    }
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> Get()
    {
      var response = await _userService.GetUsers();
      if(response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpDelete]
    public async Task<ActionResult<ServiceResponse<int>>> Delete(int userId)
    {
      var response = await _userService.Delete(userId);
      if(response.Success) return Ok(response);
      return BadRequest(response);
    }
  }
}