using DOTNET_RPG.Models;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_RPG.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbcontextOptions) : base(dbcontextOptions)
        {

        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CharacterSkill> CharacterSkills { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterSkill>(x => x.HasKey(y => new { y.CharacterId, y.SkillId }));
            modelBuilder.Entity<Character>()
            .HasMany(character => character.skills)
            .WithMany(skill => skill.characters)
            .UsingEntity<CharacterSkill>(
                characterSkill => characterSkill
                .HasOne(s => s.skill)
                .WithMany(),
                characterSkill => characterSkill
                .HasOne(c => c.character)
                .WithMany()
            );
        }
    }
}