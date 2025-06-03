using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;

namespace G_Coffee_Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<User?> LoginAsync(UserLoginDTO loginDto)
        {
            var user = await _accountRepository.GetUserByNameAsync(loginDto.Username);
            if (user == null || user.Password != loginDto.Password) // Note: In production, use proper password hashing
                return null;
            return user;
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
