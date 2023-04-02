namespace Services.Weapon
{
  public interface IWeaponService
  {
    Task<ServiceResponse<GetCharacterDto>> CreateWeapon(PostWeaponDto newWeapon);
    Task<ServiceResponse<GetCharacterDto>> DeleteWeapon(int weaponId);
  }
}