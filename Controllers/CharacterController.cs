using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _iCharacterService;
        public CharacterController(ICharacterService iCharacterService)
        {
            _iCharacterService = iCharacterService;
        }

        [Route("GetAll")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _iCharacterService.GetAllCharacters());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSingle(int Id)
        {
            return Ok(await _iCharacterService.GetById(Id));
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(CharacterDTO characterDTO)
        {
            return Ok(await _iCharacterService.AddCharacter(characterDTO));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter(CharacterDTO characterDTO)
        {
            return Ok(await _iCharacterService.UpdateCharacter(characterDTO));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCharacter(int Id)
        {
            return Ok(await _iCharacterService.DeleteCharacter(Id));
        }
    }
}