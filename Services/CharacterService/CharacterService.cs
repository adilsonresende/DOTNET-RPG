using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DOTNET_RPG.DTOs.Character;
using System.Threading.Tasks;
using System.Security.Claims;
using DOTNET_RPG.Models;
using DOTNET_RPG.Data;
using System.Linq;
using AutoMapper;
using System;

namespace DOTNET_RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _iMapper;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _iHttpContextAccessor;

        private int GetUserId()
        {
            Int32.TryParse(_iHttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int user_id);
            return user_id;
        }

        private string GetUserRole()
        {
            return _iHttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }


        public CharacterService(DataContext dataContext, IMapper iMapper, IHttpContextAccessor iHttpContextAccessor)
        {
            _iHttpContextAccessor = iHttpContextAccessor;
            _dataContext = dataContext;
            _iMapper = iMapper;
        }

        public async Task<ServiceResponse<List<CharacterDTO>>> AddCharacter(CharacterDTO characterDTO)
        {
            ServiceResponse<List<CharacterDTO>> serviceResponse = new ServiceResponse<List<CharacterDTO>>();
            try
            {
                Character character = _iMapper.Map<Character>(characterDTO);
                character.User = await _dataContext.Users.FirstAsync(x => x.Id == GetUserId());
                _dataContext.Characters.Add(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = _iMapper.Map<List<CharacterDTO>>(await _dataContext.Characters
                .Where(x => x.User.Id == GetUserId())
                .ToListAsync());
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterDTO>>> GetAllCharacters()
        {
            ServiceResponse<List<CharacterDTO>> serviceResponse = new ServiceResponse<List<CharacterDTO>>();
            try
            {
                List<Character> characters = GetUserRole() == "Admin" ?
                await _dataContext.Characters
                .Include(x => x.weapon)
                .AsNoTracking()
                .ToListAsync()
                : 
                await _dataContext.Characters
                .Include(x => x.weapon)
                .AsNoTracking()
                .Where(x => x.User.Id == GetUserId())
                .ToListAsync();

                serviceResponse.Data = _iMapper.Map<List<CharacterDTO>>(characters);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterDTO>> GetById(int Id)
        {
            ServiceResponse<CharacterDTO> serviceResponse = new ServiceResponse<CharacterDTO>();
            try
            {
                Character character = await _dataContext.Characters
                .Include(x => x.weapon)
                .FirstOrDefaultAsync(x => x.Id == Id && x.User.Id == GetUserId());
                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                    return serviceResponse;
                }

                serviceResponse.Data = _iMapper.Map<CharacterDTO>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterDTO>> UpdateCharacter(CharacterDTO characterDTO)
        {
            ServiceResponse<CharacterDTO> serviceResponse = new ServiceResponse<CharacterDTO>();
            try
            {
                Character character = await _dataContext.Characters
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == characterDTO.Id);

                if (character.User.Id == GetUserId())
                {
                    character.Name = characterDTO.Name;
                    character.HitPoints = characterDTO.HitPoints;
                    character.Strenght = characterDTO.Strenght;
                    character.Defense = characterDTO.Defense;
                    character.Intelligence = characterDTO.Intelligence;
                    character.RpgClass = characterDTO.rpgClass;

                    _dataContext.Characters.Update(character);
                    await _dataContext.SaveChangesAsync();
                    serviceResponse.Data = _iMapper.Map<CharacterDTO>(character);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CharacterDTO>>> DeleteCharacter(int Id)
        {
            ServiceResponse<List<CharacterDTO>> serviceResponse = new ServiceResponse<List<CharacterDTO>>();
            try
            {
                Character character = await _dataContext.Characters.FirstAsync(x => x.Id == Id && x.User.Id == GetUserId());
                if (character != null)
                {
                    _dataContext.Remove(character);
                    await _dataContext.SaveChangesAsync();
                }

                serviceResponse.Success = true;
                List<Character> characters = await _dataContext.Characters
                .AsNoTracking()
                .Where(x => x.User.Id == GetUserId())
                .ToListAsync();

                serviceResponse.Data = _iMapper.Map<List<CharacterDTO>>(characters);
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