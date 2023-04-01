namespace Services.Character
{
  public class CharacterService : ICharacterService
  {
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
      _context = context;
      _mapper = mapper;
    }
    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetCharacterWithUser()
    {
      var response = new ServiceResponse<List<GetCharacterDto>>();
      try
      {
        // Get characters
        var characters = await _context.Characters
          .Include(c => c.Weapon)
          .Include(c => c.Skills)
          .Where(c => c.User!.Id == GetUserId())
          .ToListAsync();
        // Create response
        response.Message = $"Characters for user with id {GetUserId()}.";
        response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterWithId(int id)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try
      {
        // Get character
        var character = await _context.Characters
        .Include(c => c.Weapon)
        .Include(c => c.Skills)
        .FirstOrDefaultAsync(c => c.Id == id);
        if (character is null) throw new Exception($"Character with id {id} not found.");
        // Create response
        response.Message = $"This is the character with id {id}.";
        response.Data = _mapper.Map<GetCharacterDto>(character);
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<List<GetCharacterDto>>> CreateCharacter(PostCharacterDto newCharacter)
    {
      var response = new ServiceResponse<List<GetCharacterDto>>();
      try
      {
        // Create character
        var character = _mapper.Map<Models.Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        // Save character
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        // Create response
        response.Message = $"The characters that belongs to user with id {GetUserId()}.";
        response.Data = await _context.Characters
          .Where(c => c.User!.Id == GetUserId())
          .Select(c => _mapper.Map<GetCharacterDto>(c))
          .ToListAsync();
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
      var response = new ServiceResponse<List<GetCharacterDto>>();
      try
      {
        // Get character
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
        if (character is null) throw new Exception($"Character with Id '{id}' not found.");
        // Save
        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();
        // Create response
        response.Message = "All characters.";
        response.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(PutCharacterDto updatedCharacter)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try
      {
        // Get character
        var character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
        if (character is null) throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
        // Alter character
        character.Strength = updatedCharacter.Strength;
        character.Defense = updatedCharacter.Defense;
        character.Intelligence = updatedCharacter.Intelligence;
        character.Class = updatedCharacter.Class;
        // Save
        await _context.SaveChangesAsync();
        // Create response
        response.Message = "Character with new updates.";
        response.Data = _mapper.Map<GetCharacterDto>(character);
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<GetCharacterDto>> GiveSkillToCharacter(PostCharacterSkillDto newCharacterSkill)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try
      {
        // Get Character
        var character = await _context.Characters
          .Include(c => c.Weapon)
          .Include(c => c.Skills)
          .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId);
        if (character is null) throw new Exception("Character not found.");
        // Get Skill
        var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
        if (skill is null) throw new Exception("Skill not found.");
        // Create Character-Skill connection
        character.Skills!.Add(skill);
        await _context.SaveChangesAsync();
        // Create response
        response.Message = "Character with new skill.";
        response.Data = _mapper.Map<GetCharacterDto>(character);
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
  }
}