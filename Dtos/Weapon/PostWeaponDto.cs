namespace Dtos.Weapon
{
  public class PostWeaponDto
  {
    public string Name { get; set; } = string.Empty;
    public int Damage { get; set; }
    public int CharacterId { get; set; }
  }
}