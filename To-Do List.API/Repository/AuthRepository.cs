using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using To_Do_List.API.Data;
using To_Do_List.API.Models;

namespace To_Do_List.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApiDBContext _context;
        private readonly ILogger<AuthRepository> _logger;
        private readonly IConfiguration _configuration;

        public AuthRepository(ApiDBContext context, ILogger<AuthRepository> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<User?> AuthenticateUser(string username, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                    return null;

                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return null;

                _logger.LogInformation($"User with username '{user.Username}' logged in successfully.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error authenticating user: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsUsernameUnique(string username) =>
            !await _context.Users.AnyAsync(u => u.Username == username);

        public async Task<User?> Register(UserDTO user)
        {
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

                var newUser = new User()
                {
                    Username = user.Username,
                    PasswordHash = passwordHash
                };

                await _context.Users.AddAsync(newUser);

                await _context.SaveChangesAsync();
                _logger.LogInformation($"User with username '{user.Username}' registered successfully.");

                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering user: {ex.Message}");
                return null;
            }
        }


        public string CreateToken(User user) // TODO: Сделать рефреш токен и уменьшить время действия токена
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:KeyToken").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        public async Task<User?> GetUser(string id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id.ToString() == id)
                    .Select(u => new User
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Tasks = u.Tasks,
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogInformation($"User with id {id} not found.");
                    return null;
                }

                _logger.LogInformation($"User with id {id} found.");

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting user with id {id}: {ex.Message}");
                return null;
            }
        }
    }
}
