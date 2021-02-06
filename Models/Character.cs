namespace DOTNET_RPG.Models
{
    public class Character
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HitPoints { get; set; }

        public int Strenght { get; set; }

        public int Defense { get; set; }

        public int Intelligence { get; set; }

        public RpgClass RpgClass { get; set; } = RpgClass.Knight;
    }
}