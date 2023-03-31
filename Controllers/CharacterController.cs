namespace Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class CharacterController : ControllerBase
  {
    private readonly ICharacterService _characterService;
    public CharacterController(ICharacterService characterService)
    {
      _characterService = characterService;
    }
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
    {
      return Ok(await _characterService.GetCharacterWithUser());
    }
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get(int id)
    {
      return Ok(await _characterService.GetCharacterWithId(id));
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Post(AddCharacterDto newCharacter)
    {
      return Ok(await _characterService.CreateCharacter(newCharacter));
    }
    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
      var response = await _characterService.DeleteCharacter(id);
      if (response.Data is null)
      {
        return NotFound(response);
      }
      return Ok(response);
    }
    [AllowAnonymous]
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var response = await _characterService.UpdateCharacter(updatedCharacter);
      if (response.Data is null)
      {
        return NotFound(response);
      }
      return Ok(response);
    }
    [HttpPost("Skill")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
      return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
    }
  }
}