namespace Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeaponController : ControllerBase
  {
    private readonly IWeaponService _weaponService;
    public WeaponController(IWeaponService weaponService)
    {
      _weaponService = weaponService;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Post(PostWeaponDto newWeapon)
    {
      var response = await _weaponService.CreateWeapon(newWeapon);
      if(response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpDelete]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Delete(int weaponId)
    {
      var response = await _weaponService.DeleteWeapon(weaponId);
      if(response.Success) return Ok(response);
      return BadRequest(response);
    }
  }
}