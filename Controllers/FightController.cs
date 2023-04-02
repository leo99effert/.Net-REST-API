namespace Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class FightController : ControllerBase
  {
    private readonly IFightService _fightService;
    public FightController(IFightService fightService)
    {
      _fightService = fightService;
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FightResultDto>>> Post(FightRequestDto request)
    {
      var response = await _fightService.Fight(request);
      if(response.Success) return Ok(response);
      return BadRequest(response);
    }
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<HighScoreDto>>>> Get()
    {
      var response = await _fightService.ShowHighScore();
      if(response.Success) return Ok(response);
      return BadRequest(response);
    }
  }
}