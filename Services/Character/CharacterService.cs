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
          .Include(c => c.Weapon)
          .Include(c => c.Skills)
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
        response.Data = await _context.Characters.Include(c => c.Weapon).Include(c => c.Skills).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<List<GetCharacterDto>>> ResetCharacters()
    {
      var response = new ServiceResponse<List<GetCharacterDto>>();
      try
      {
        // Get characters
        var characters = await _context.Characters
          .Include(c => c.Weapon)
          .Include(c => c.Skills)
          .ToListAsync();
        // Reset characters
        foreach (var character in characters)
        {
          character.Strength = 10;
          character.Defense = 10;
          character.Intelligence = 10;
          if (character.Weapon is not null) _context.Weapons.Remove(character.Weapon);
          _context.Weapons.Add( new Models.Weapon { Character = character, Name = "Basic Sword", Damage = 20});
          character.Skills!.Clear();
          var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == 1);
          character.Skills.Add(skill!);
          await _context.SaveChangesAsync();
        }
        // Create response
        response.Message = $"Characters with standard values";
        response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
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
        var character = await _context.Characters.Include(c => c.Weapon).Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
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
    public async Task<ServiceResponse<GetCharacterDto>> UpdateSkills(PutSkillsDto newSkills)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try
      {
        // Get Character
        var character = await _context.Characters
          .Include(c => c.Weapon)
          .Include(c => c.Skills)
          .FirstOrDefaultAsync(c => c.Id == newSkills.CharacterId);
        if (character is null) throw new Exception("Character not found.");
        // Get Skills
        var skills = await _context.Skills.Where(s => newSkills.Skills.Contains(s.Id)).ToListAsync();
        // Create Character-Skill connection
        character.Skills!.Clear();
        foreach (var skill in skills) character.Skills!.Add(skill);
        await _context.SaveChangesAsync();
        // Create response
        response.Message = "Character with new skills.";
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