using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Weapon;
using DOTNET_RPG.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _iWeaponService;

        public WeaponController(IWeaponService iWeaponService)
        {
            _iWeaponService = iWeaponService;
        }

        [HttpPost]
        public async Task<IActionResult> AddWeapon(WeaponDTO weaponDTO)
        {
            return Ok(await _iWeaponService.AddWeapon(weaponDTO));
        }
    }
}