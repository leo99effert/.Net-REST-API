namespace Dtos.User
{
  public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<GetMinimalCharacterDto>? Characters { get; set; }
    }
}