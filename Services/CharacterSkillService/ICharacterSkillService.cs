using DOTNET_RPG.DTOs.CharacterSkillDTO;
using DOTNET_RPG.DTOs.Character;
using System.Threading.Tasks;
using DOTNET_RPG.Models;

namespace DOTNET_RPG.Services.CharacterSkillService
{
    public interface ICharacterSkillService
    {
        Task<ServiceResponse<CharacterDTO>> AddCharacterSkill(CharacterSkillDTO characterSkillDTO);
    }
}