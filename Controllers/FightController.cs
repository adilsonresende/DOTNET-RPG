using AutoMapper;
using DOTNET_RPG.Services.FightService;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _iFightService;
        private readonly IMapper _imapper;
        public FightController(IFightService iFightService, IMapper imapper)
        {
            _imapper = imapper;
            _iFightService = iFightService;
        }
    }
}