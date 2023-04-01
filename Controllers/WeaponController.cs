namespace Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class WeaponController : ControllerBase
  {
    private readonly IWeaponService _weaponService;
    public WeaponController(IWeaponService weaponService)
    {
      _weaponService = weaponService;
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Post(PostWeaponDto newWeapon)
    {
      return Ok(await _weaponService.CreateWeapon(newWeapon));
    }
  }
}