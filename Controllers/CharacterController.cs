namespace Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CharacterController : ControllerBase
  {
    private readonly ICharacterService _characterService;
    public CharacterController(ICharacterService characterService)
    {
      _characterService = characterService;
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
    {
      return Ok(await _characterService.GetCharacterWithUser());
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get(int id)
    {
      return Ok(await _characterService.GetCharacterWithId(id));
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Post(AddCharacterDto newCharacter)
    {
      return Ok(await _characterService.CreateCharacter(newCharacter));
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
      return Ok(await _characterService.DeleteCharacter(id));
    }
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Put(UpdateCharacterDto updatedCharacter)
    {
      return Ok(await _characterService.UpdateCharacter(updatedCharacter));
    }
    [HttpPost("Skill")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
      return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
    }
  }
}