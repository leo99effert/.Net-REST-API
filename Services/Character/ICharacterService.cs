namespace Services.Character
{
  public interface ICharacterService
  {
    Task<ServiceResponse<List<GetCharacterDto>>> GetCharacterWithUser();
    Task<ServiceResponse<GetCharacterDto>> GetCharacterWithId(int id);
    Task<ServiceResponse<List<GetCharacterDto>>> CreateCharacter(PostCharacterDto newCharacter);
    Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
    Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(PutCharacterDto updatedCharacter);
    Task<ServiceResponse<GetCharacterDto>> UpdateSkills(PutSkillsDto newSkills);
  }
}