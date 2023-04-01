namespace Services.Weapon
{
  public interface IWeaponService
  {
    Task<ServiceResponse<GetCharacterDto>> CreateWeapon(PostWeaponDto newWeapon);
  }
}