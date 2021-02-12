namespace DOTNET_RPG.DTOs.Weapon
{
    public class WeaponDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Damage { get; set; }

        public Models.Character character { get; set; }

        public int CharacterId { get; set; }
    }
}