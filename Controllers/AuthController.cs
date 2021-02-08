using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.User;
using DOTNET_RPG.Models;
using DOTNET_RPG.Data;

namespace DOTNET_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _iAuthRepository;
        public AuthController(IAuthRepository iAuthRepository)
        {
            _iAuthRepository = iAuthRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            ServiceResponse<int> serviceResponse = await _iAuthRepository.Register(
                new User { Username = userRegisterDTO.Username }, userRegisterDTO.Password);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRegisterDTO userRegisterDTO)
        {
            ServiceResponse<string> serviceResponse = await _iAuthRepository.Login(userRegisterDTO.Username, userRegisterDTO.Password);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }
    }
}