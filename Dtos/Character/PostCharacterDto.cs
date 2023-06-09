namespace Dtos.Character
{
  public class PostCharacterDto
  {
    public string Name { get; set; } = "Frodo";
    public int Strength { get; set; } = 10;
    public int Defense { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    public RpgClass Class { get; set; } = RpgClass.Knight;
  }
}