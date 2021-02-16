using System.Collections.Generic;
using DOTNET_RPG.DTOs.Skill;
using DOTNET_RPG.DTOs.Weapon;
using DOTNET_RPG.Models;

namespace DOTNET_RPG.DTOs.Character
{
    public class CharacterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int Strenght { get; set; }
        public int Defense { get; set; }
        public int Intelligence { get; set; }
        public RpgClass rpgClass { get; set; }
        public WeaponDTO weaponDTO { get; set; }
        public List<SkillDTO> skillDTOs { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}