namespace Dtos.Character
{
  public class PutSkillsDto
  {
    public int CharacterId { get; set; }
    public List<int> Skills { get; set; } = new List<int>();
  }
}