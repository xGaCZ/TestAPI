using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestNetAPI.Entities;
using TestNetAPI.Exceptions;
using TestNetAPI.Models;

namespace TestNetAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService :IAccountService
    {
        private readonly NetDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(NetDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        { 
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            if(!IsPasswordComplexEnough(dto.Password))
            {
                throw new BadRequestException("Hasło nie spełnia wymagań złożoności.");
            }
            var newUser = new User()
            {
                Email = dto.Email,
                RoleId = dto.RoleId,
            };
          var hashedPassword =  _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto) //generowanie tokenu dla poprawnie zalogowanego użytkownika 
        { 
       var user = _context.Users.Include(x => x.Role)
                .FirstOrDefault(u=> u .Email == dto.Email);

            if (user is null) 
            {
                throw new BadRequestException("niepoprawne dane ");
            }
          var result = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash,dto.Password);
            if(result == PasswordVerificationResult.Failed) 
            {
                throw new BadRequestException("niepoprawne dane ");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Role,$"{user.Role.Name}"),
             
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred =new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,_authenticationSettings.JwtIssuer,claims,expires:expires,signingCredentials:cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }

        private bool IsPasswordComplexEnough(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }
            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower))
            {
                return false;
            }     

            return true;
        }
        
    }
}
