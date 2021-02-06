using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DOTNET_RPG.DTOs.Character;
using System.Threading.Tasks;
using DOTNET_RPG.Models;
using DOTNET_RPG.Data;
using AutoMapper;
using System;

namespace DOTNET_RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _iMapper;
        private readonly DataContext _dataContext;

        public CharacterService(DataContext dataContext, IMapper iMapper)
        {
            _dataContext = dataContext;
            _iMapper = iMapper;
        }

        public async Task<ServiceResponse<List<CharacterDTO>>> AddCharacter(CharacterDTO characterDTO)
        {
            ServiceResponse<List<CharacterDTO>> serviceResponse = new ServiceResponse<List<CharacterDTO>>();
            try
            {
                _dataContext.Characters.Add(_iMapper.Map<Character>(characterDTO));
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = _iMapper.Map<List<CharacterDTO>>(await _dataContext.Characters.ToListAsync());
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
                serviceResponse.Data = _iMapper.Map<List<CharacterDTO>>(await _dataContext.Characters.ToListAsync());
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
                serviceResponse.Data = _iMapper.Map<CharacterDTO>(await _dataContext.Characters.FirstOrDefaultAsync(x => x.Id == Id));
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
                Character character = await _dataContext.Characters.FirstOrDefaultAsync(x => x.Id == characterDTO.Id);
                character.Name = characterDTO.Name;
                character.HitPoints = characterDTO.HitPoints;
                character.Strenght = characterDTO.Strenght;
                character.Defense = characterDTO.Defense;
                character.Intelligence = characterDTO.Intelligence;
                character.RpgClass = characterDTO.RpgClass;

                _dataContext.Characters.Update(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = _iMapper.Map<CharacterDTO>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<CharacterDTO>> DeleteCharacter(int Id)
        {
            ServiceResponse<CharacterDTO> serviceResponse = new ServiceResponse<CharacterDTO>();
            try
            {
                Character character = await _dataContext.Characters.FirstAsync(x => x.Id == Id);
                _dataContext.Remove(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Success = true;
                serviceResponse.Data = _iMapper.Map<CharacterDTO>(character);
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