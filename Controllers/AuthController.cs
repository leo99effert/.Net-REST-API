global using Microsoft.AspNetCore.Mvc;
global using Dtos.User;

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

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
    {
      var response = await _authrepo.Register(
          new User { Username = request.Username }, request.Password
      );
      if (!response.Success)
      {
        return BadRequest(response);
      }
      return Ok(response);
    }
    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
    {
      var response = await _authrepo.Login(request.Username, request.Password);
      if (!response.Success)
      {
        return BadRequest(response);
      }
      return Ok(response);
    }
    [HttpDelete]
    public async Task<ActionResult<ServiceResponse<int>>> Delete(int userId)
    {
      var response = await _authrepo.Delete(userId);
      return Ok(response);
    }
  }
}