using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("GetAll")]
        public ActionResult<List<Character>> Get() 
        {
            return Ok(_characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public ActionResult<List<Character>> GetSingle(int id) 
        {
            return Ok(_characterService.GetCharacterById(id));
        }

        [HttpPost]
        public ActionResult<List<Character>> AddCharacter(Character newCharacter) 
        {
            return Ok(_characterService.AddCharacter(newCharacter));
        }
    }
}