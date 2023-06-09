namespace _Net_REST_API
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<Character, GetCharacterDto>();
      CreateMap<PostCharacterDto, Character>();
      CreateMap<Weapon, GetWeaponDto>();
      CreateMap<Skill, GetSkillDto>();
      CreateMap<Character, HighScoreDto>();
      CreateMap<User, GetUserDto>();
      CreateMap<Character, GetMinimalCharacterDto>();
    }
  }
}