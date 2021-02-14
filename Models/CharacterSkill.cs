using System.Collections.Generic;

namespace DOTNET_RPG.Models
{
    public class CharacterSkill
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
        public Character character { get; set; }
        public Skill skill { get; set; }
    }
}