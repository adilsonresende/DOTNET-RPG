using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DOTNET_RPG.Data;
using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.DTOs.Weapon;
using DOTNET_RPG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_RPG.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _iHttpContextAccessor;

        public WeaponService(DataContext dataContext, IHttpContextAccessor iHttpContextAccessor)
        {
            _iHttpContextAccessor = iHttpContextAccessor;

        }

        private int GetUserId()
        {
            Int32.TryParse(_iHttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int user_id);
            return user_id;
        }

        public async Task<ServiceResponse<CharacterDTO>> AddWeapon(WeaponDTO weaponDTO)
        {
            ServiceResponse<CharacterDTO> serviceResponse = new ServiceResponse<CharacterDTO>();
            try
            {
                Character character = await _dataContext.Characters.FirstOrDefaultAsync(x =>
                x.Id == weaponDTO.CharacterId && x.User.Id == GetUserId());

                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                }

                Weapon weapon = new Weapon
                {
                    Name = weaponDTO.Name,
                    Damage = weaponDTO.Damage,
                    CharacterId = character.User.Id
                };

                await _dataContext.Weapons.AddAsync(weapon);
                await _dataContext.SaveChangesAsync();

                
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}