using System.Collections.Generic;
using DOTNET_RPG.DTOs.Character;
using System.Threading.Tasks;
using DOTNET_RPG.Models;

namespace DOTNET_RPG.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<CharacterDTO>>> GetAllCharacters();

        Task<ServiceResponse<CharacterDTO>> GetById(int Id);

        Task<ServiceResponse<List<CharacterDTO>>> AddCharacter(CharacterDTO characterDTO);

        Task<ServiceResponse<CharacterDTO>> UpdateCharacter(CharacterDTO characterDTO);

        Task<ServiceResponse<List<CharacterDTO>>> DeleteCharacter(int Id);
    }
}