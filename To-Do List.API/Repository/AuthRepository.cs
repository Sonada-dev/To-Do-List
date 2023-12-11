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
                {
                    _logger.LogInformation($"Authentication failed. User with username '{username}' not found.");
                    return null;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    _logger.LogInformation($"Authentication failed. Incorrect password for user with username '{username}'.");
                    return null;
                }

                _logger.LogInformation($"User with username '{user.Username}' logged in successfully.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error authenticating user: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsUsernameUnique(string username)
        {
            try
            {
                var isUnique = !await _context.Users.AnyAsync(u => u.Username == username);
                _logger.LogInformation($"Checking uniqueness of username '{username}': {isUnique}");
                return isUnique;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking uniqueness of username: {ex.Message}");
                return false;
            }
        }

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

        public string? CreateToken(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:KeyToken").Value!));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(1), signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating token: {ex.Message}");
                return null;
            }
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

        public RefreshToken? GenerateRefreshToken()
        {
            try
            {
                var refreshToken = new RefreshToken
                {
                    Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                    Exprires = DateTime.Now.AddDays(7)
                };

                return refreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating refresh token: {ex.Message}");
                return null;
            }
        }

        public CookieOptions? SetCookieOptionsForToken(RefreshToken refreshToken)
        {
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = refreshToken.Exprires,
                };

                return cookieOptions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting cookie options: {ex.Message}");
                return null;
            }
        }

        public async Task SetRefreshToken(RefreshToken refreshToken, Guid userId)
        {
            try
            {
                var user = await _context.Users.FirstAsync(x => x.Id == userId);

                if (user != null)
                {
                    user.TokenCreated = refreshToken.Created.ToUniversalTime();
                    user.TokenExpires = refreshToken.Exprires.ToUniversalTime();
                    user.RefreshToken = refreshToken.Token;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting refresh token: {ex.Message}");
            }
        }
    }
}
