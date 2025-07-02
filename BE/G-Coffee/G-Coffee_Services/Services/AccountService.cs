using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
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
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _accountRepository = accountRepository;
            _mapper = mapper;
            _jwtSecret = _configuration["JwtConfig:Key"] ?? throw new ArgumentNullException("JwtConfig" + ":Key", "JWT secret key is not configured");


        }

        public async Task<LoginResponseDTO?> LoginAsync(UserLoginDTO loginDto)
        {
            var user = await _accountRepository.GetUserByNameAsync(loginDto.Username);
            if (user == null || user.Password != loginDto.Password) // Note: Use proper password hashing in production
                throw new UnauthorizedAccessException("Invalid credentials");

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMinues = _configuration.GetValue<int>("JwtConfig:TokenValidityMinutes", 30);
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMinues);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()) // Assuming User has a Role property
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                , SecurityAlgorithms.HmacSha256Signature),
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler(); // Renamed variable to avoid conflict
            var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            var assertedToken = jwtTokenHandler.WriteToken(securityToken);

            return new LoginResponseDTO
            {
                AccessToken = assertedToken,
                Username = loginDto.Username,
                ExpiresIn = (int)(tokenExpiryTimeStamp - DateTime.UtcNow).TotalSeconds,
                Role = user.Role.ToString() // Fix: Assign the RoleEnum directly instead of converting to string
            };
        }
        public async Task RegisterAsync(UserRegisterDTO registerDto)
        {
            var existingUser = await _accountRepository.GetUserByNameAsync(registerDto.Username);
            if (existingUser != null)
                throw new Exception("Username already exists");

            var user = _mapper.Map<User>(registerDto);
            await _accountRepository.AddUserAsync(user);
        }

        public async Task DeleteAccountAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Account ID is required");

            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new KeyNotFoundException($"Account with ID {id} not found");

            _accountRepository.Remove(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null || !accounts.Any())
                throw new KeyNotFoundException("No accounts found");

            return accounts;
        }

        public async Task<User> GetAccountByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Account ID is required");

            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new KeyNotFoundException($"Account with ID {id} not found");

            return account;
        }

        public async Task UpdateAccountAsync(string id, UserUpdateDTO dto)
        {
            var existing = await _accountRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Account with ID {id} not found");

            // Cập nhật thủ công hoặc dùng AutoMapper
            _mapper.Map(dto, existing);

            _accountRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
