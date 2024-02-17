using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatrickGodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]//
    /* 
     * Before using a class as a controller we need to make it a proper controller. For this we have to derive it from
     * ControllerBase which is a base class for MVC controller without view support.
     * IActionResult enables us to send other data e.g. status codes alongwith actual requested data. So Ok(knight) will
     * send knight (the data which was requested) and status code (Ok).
     * Actually WebAPI supports naming convention. So when a method starts with 'Get' it will be treated as HttpGet method. 
     * However swagger requires that it should explicitly be decorated with HttpGet.
     *      The GET (Read) method requests a representation of the specified resource. We can not submit an entity in GET.
     *      The POST (Create) method is used to submit an entity to the specified resource, often causing a change in state or 
     *      side effects on the server
     *      The PUT (Update) method replaces all current representations of the target resource with the request payload.
     *      The DELETE (Delete) method deletes a specified resource. 
     * WebAPI Best Practices:
     *      Client sends request to controller through DTO
     *      Controller sends request to service and ask for data using DTO
     *      Service uses Models and ask data from Database
     *      Database return data in Model form
     *      Service maps model to DTO and sends to controller
     *      Controller sends data in form of DTO to Client
    */
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService characterService;
        public CharacterController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost] 
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto character) 
        {
            return Ok(await characterService.AddCharacter(character));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await characterService.UpdateCharacter(updatedCharacter);

            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var response = await characterService.DeleteCharacter(id);

            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
