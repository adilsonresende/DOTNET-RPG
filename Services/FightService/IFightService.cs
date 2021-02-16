using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Fight;
using DOTNET_RPG.Models;

namespace DOTNET_RPG.Services.FightService
{
    public interface IFightService
    {
         public Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO weaponAttackDTO);

         public Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO skillAttackDTO);
    }
}