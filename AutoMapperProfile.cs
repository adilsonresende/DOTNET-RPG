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
            CreateMap<Character, CharacterDTO>()
                .ForMember(dest => dest.skillDTOs, src => src.MapFrom(x => x.skills))
                .ForMember(dest => dest.weaponDTO, src => src.MapFrom(x => x.weapon));
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