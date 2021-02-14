using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DOTNET_RPG.Data;
using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.DTOs.CharacterSkillDTO;
using DOTNET_RPG.DTOs.Skill;
using DOTNET_RPG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_RPG.Services.CharacterSkillService
{
    public class CharacterSkillService : ICharacterSkillService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _iMapper;
        private readonly IHttpContextAccessor _iHttpContextAccessor;
        public CharacterSkillService(DataContext dataContext, IMapper iMapper, IHttpContextAccessor iHttpContextAccessor)
        {
            _iHttpContextAccessor = iHttpContextAccessor;
            _iMapper = iMapper;
            _dataContext = dataContext;
        }

        private int GetUserId()
        {
            Int32.TryParse(_iHttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int user_id);
            return user_id;
        }

        public async Task<ServiceResponse<CharacterDTO>> AddCharacterSkill(CharacterSkillDTO characterSkillDTO)
        {
            ServiceResponse<CharacterDTO> ServiceResponse = new ServiceResponse<CharacterDTO>();
            try
            {
                CharacterSkill characterSkill = await _dataContext.CharacterSkills.FirstOrDefaultAsync(x => x.CharacterId == characterSkillDTO.characterId && x.SkillId == characterSkillDTO.skillId);
                if (characterSkill != null)
                {
                     ServiceResponse.Success = false;
                     ServiceResponse.Message = "Character alread has skill."   ;
                     return ServiceResponse;
                }

                CharacterSkill characterSkillEntity = _iMapper.Map<CharacterSkill>(characterSkillDTO);
                await _dataContext.CharacterSkills.AddAsync(characterSkillEntity);
                await _dataContext.SaveChangesAsync();

                Character character = await _dataContext.Characters
                .Include(x => x.weapon)
                .Include(x => x.User)
                .Include(x => x.skills)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == characterSkillDTO.characterId && x.User.Id == GetUserId());
                
                CharacterDTO characterDTO = _iMapper.Map<CharacterDTO>(character);
                ServiceResponse.Data = characterDTO;

            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }

            return ServiceResponse;
        }
    }
}