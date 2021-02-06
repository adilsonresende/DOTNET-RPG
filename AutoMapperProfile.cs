using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.Models;
using AutoMapper;

namespace DOTNET_RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, CharacterDTO>();
            CreateMap<CharacterDTO, Character>();
        }
    }
}