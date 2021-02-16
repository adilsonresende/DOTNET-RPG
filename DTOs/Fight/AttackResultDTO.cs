namespace DOTNET_RPG.DTOs.Fight
{
    public class AttackResultDTO
    {
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public int AttackerHP { get; set; }
        public int DefenderHP { get; set; }
        public int Damage { get; set; }
    }
}