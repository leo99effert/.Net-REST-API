namespace Services.Weapon
{
  public class WeaponService : IWeaponService
  {
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _context = context;
    }
    public async Task<ServiceResponse<GetCharacterDto>> CreateWeapon(PostWeaponDto newWeapon)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try
      {
        // Get user
        int userId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User!.Id == userId);
        if (character is null) throw new Exception($"Character with id {newWeapon.CharacterId} not found on user with id {userId}.");
        // Save
        var weapon = new Models.Weapon { Name = newWeapon.Name, Damage = newWeapon.Damage, Character = character };
        _context.Weapons.Add(weapon);
        await _context.SaveChangesAsync();
        // Create response
        response.Message = $"The weapon {newWeapon.Name} was created and added to the character with id {newWeapon.CharacterId}";
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