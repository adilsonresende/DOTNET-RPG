using DOTNET_RPG.DTOs.Character;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Weapon;
using DOTNET_RPG.Models;

namespace DOTNET_RPG.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<CharacterDTO>> AddWeapon(WeaponDTO weaponDTO);
    }
}