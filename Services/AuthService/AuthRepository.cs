using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using DOTNET_RPG.Utility;
using DOTNET_RPG.Models;
using DOTNET_RPG.Data;
using System.Text;
using System;

namespace DOTNET_RPG.Services.AuthService
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _iConfiguration;
        public AuthRepository(DataContext dataContext, IConfiguration iConfiguration)
        {
            _dataContext = dataContext;
            _iConfiguration = iConfiguration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                User user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                if (user == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found.";
                    return serviceResponse;
                }
                else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Invalid password.";
                    return serviceResponse;
                }

                serviceResponse.Success = true;
                serviceResponse.Data = CreateToken(user);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> serviceResponse = new ServiceResponse<int>();
            try
            {
                if (await UserExists(user.Username))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User alread exists.";
                    return serviceResponse;
                }

                PasswordHashHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _dataContext.Users.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = user.Id;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passowordSalt)
        {
            using (var hmacsha = new System.Security.Cryptography.HMACSHA512(passowordSalt))
            {
                var ComputeHash = hmacsha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < ComputeHash.Length; i++)
                {
                    if (ComputeHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dataContext.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }
            return false;
        }

        private string CreateToken(User user)
        {
            List<Claim> claim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            SymmetricSecurityKey systemSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_iConfiguration.GetSection("AppSettings").GetSection("Token").Value)
            );

            SigningCredentials signingCredentials = new SigningCredentials(systemSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}