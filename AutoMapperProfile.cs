using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.Models;
using AutoMapper;
using DOTNET_RPG.DTOs.Weapon;
using DOTNET_RPG.DTOs.CharacterSkillDTO;
using DOTNET_RPG.DTOs.Skill;

namespace DOTNET_RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, CharacterDTO>();
            CreateMap<CharacterDTO, Character>();
            CreateMap<Weapon, WeaponDTO>();
            CreateMap<WeaponDTO, Weapon>();
            CreateMap<CharacterSkill, CharacterSkillDTO>();
            CreateMap<CharacterSkillDTO, CharacterSkill>();
            CreateMap<Skill, SkillDTO>();
            CreateMap<SkillDTO, Skill>();
        }
    }
}