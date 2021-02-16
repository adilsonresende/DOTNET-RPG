using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Fight;
using DOTNET_RPG.Models;
using DOTNET_RPG.Data;
using System.Linq;
using System;

namespace DOTNET_RPG.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;
        public FightService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO skillAttackDTO)
        {
            ServiceResponse<AttackResultDTO> serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                Character CharacterAttacker = await _dataContext.Characters
                    .Include(x => x.skills)
                    .FirstOrDefaultAsync(x => x.Id == skillAttackDTO.AttackerId);

                Character CharacterDefender = await _dataContext.Characters
                .Include(x => x.skills)
                .FirstOrDefaultAsync(x => x.Id == skillAttackDTO.DefenderId);

                Skill skill = CharacterAttacker.skills.FirstOrDefault(x => x.Id == skillAttackDTO.SkillId);
                if (skill == null)
                {
                    serviceResponse.Message = $"{CharacterAttacker.Name} doesn't have that skill.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                int damage = skill.Damage + (new Random().Next(CharacterAttacker.Intelligence * 2));
                damage -= new Random().Next(CharacterDefender.Defense);
                ProcessFight(damage, CharacterAttacker, CharacterDefender, serviceResponse);

                _dataContext.Characters.Update(CharacterAttacker);
                _dataContext.Characters.Update(CharacterDefender);
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = CharacterAttacker.Name,
                    Defender = CharacterDefender.Name,
                    AttackerHP = CharacterAttacker.HitPoints,
                    DefenderHP = CharacterDefender.HitPoints,
                    Damage = damage
                };

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO fightDTO)
        {
            ServiceResponse<AttackResultDTO> serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                Character CharacterAttacker = await _dataContext.Characters
                    .Include(x => x.weapon)
                    .FirstOrDefaultAsync(x => x.Id == fightDTO.AttackerId);

                Character CharacterDefender = await _dataContext.Characters
                .Include(x => x.weapon)
                .FirstOrDefaultAsync(x => x.Id == fightDTO.DefenderId);

                int damage = CharacterAttacker.weapon.Damage + (new Random().Next(CharacterAttacker.Strenght));
                damage -= new Random().Next(CharacterDefender.Defense);
                ProcessFight(damage, CharacterAttacker, CharacterDefender, serviceResponse);

                _dataContext.Characters.Update(CharacterAttacker);
                _dataContext.Characters.Update(CharacterDefender);
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = CharacterAttacker.Name,
                    Defender = CharacterDefender.Name,
                    AttackerHP = CharacterAttacker.HitPoints,
                    DefenderHP = CharacterDefender.HitPoints,
                    Damage = damage
                };

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        private void ProcessFight(int damage, Character CharacterAttacker, Character CharacterDefender, ServiceResponse<AttackResultDTO> serviceResponse)
        {
            try
            {
                CharacterAttacker.Fights++;
                CharacterDefender.Fights++;

                if (damage > 0)
                {
                    CharacterDefender.HitPoints -= damage;
                }

                if (CharacterDefender.HitPoints > 0)
                {
                    serviceResponse.Message = $"{CharacterDefender.Name} took {damage}!";
                }
                else
                {
                    CharacterAttacker.Victories++;
                    CharacterDefender.Defeats++;
                    serviceResponse.Message = $"{CharacterDefender.Name} has been defeated!";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
        }
    }
}