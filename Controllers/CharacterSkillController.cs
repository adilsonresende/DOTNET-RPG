using DOTNET_RPG.Services.CharacterSkillService;
using Microsoft.AspNetCore.Authorization;
using DOTNET_RPG.DTOs.CharacterSkillDTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DOTNET_RPG.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterSkillController : ControllerBase
    {
        private readonly ICharacterSkillService _iCharacterSkillService;
        public CharacterSkillController(ICharacterSkillService iCharacterSkillService)
        {
            _iCharacterSkillService = iCharacterSkillService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacterSkill(CharacterSkillDTO characterSkillDTO)
        {
            return Ok(await _iCharacterSkillService.AddCharacterSkill(characterSkillDTO));
        }
    }
}