using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly string _jwtSecret;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _jwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret not configured");
        }

        public async Task<string> LoginAsync(UserLoginDTO loginDto)
        {
            var user = await _accountRepository.GetUserByNameAsync(loginDto.Username);
            if (user == null || user.Password != loginDto.Password) // Note: Use proper password hashing in production
                throw new UnauthorizedAccessException("Invalid credentials");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task RegisterAsync(UserRegisterDTO registerDto)
        {
            var existingUser = await _accountRepository.GetUserByNameAsync(registerDto.Username);
            if (existingUser != null)
                throw new Exception("Username already exists");

            var user = _mapper.Map<User>(registerDto);
            await _accountRepository.AddUserAsync(user);
        }
    }
}