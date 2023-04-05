namespace Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CharacterController : ControllerBase  // Add a reset method
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
      var response = await _characterService.GetCharacterWithUser();
      if (response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get(int id)
    {
      var response = await _characterService.GetCharacterWithId(id);
      if (response.Success) return Ok(response);
      return BadRequest(response);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Post(PostCharacterDto newCharacter)
    {
      var response = await _characterService.CreateCharacter(newCharacter);
      if (response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
      var response = await _characterService.DeleteCharacter(id);
      if (response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpPut("Reset")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Reset()
    {
      var response = await _characterService.ResetCharacters();
      if(response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Put(PutCharacterDto updatedCharacter)
    {
      var response = await _characterService.UpdateCharacter(updatedCharacter);
      if (response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpPut("Skill")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> PostCharacterSkill(PutSkillsDto newSkills)
    {
      var response = await _characterService.UpdateSkills(newSkills);
      if (response.Success) return Ok(response);
      return BadRequest(response);
    }
  }
}