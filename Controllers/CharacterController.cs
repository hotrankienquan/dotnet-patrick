using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace dotnet_patrick.Controllers
{   
    //ke thua tu abstract class ControllerBase
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
    private readonly ICharacterService _characterService;
        
        public CharacterController(ICharacterService characterService)
        {
        _characterService = characterService;
            
        }
        
        [HttpGet("laytatca")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
          return Ok(await _characterService.GetAllCharacters());
        }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
      return Ok(await _characterService.GetCharacterById(id));
    }


    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto addCharacter)
        {
      return Ok(await _characterService.AddCharacter(addCharacter));
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacterDto)
    {
      var response = await _characterService.UpdateCharacter(updatedCharacterDto);

      if(response.Data is null)
      {

        return NotFound(response);
      }

      return Ok(response);
    }

     [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}