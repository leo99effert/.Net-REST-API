namespace Dtos.Character
{
  public class PutCharacterDto
  {
    public int Id { get; set; }
    public int Strength { get; set; } = 10;
    public int Defense { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    public RpgClass Class { get; set; } = RpgClass.Knight;
  }
}