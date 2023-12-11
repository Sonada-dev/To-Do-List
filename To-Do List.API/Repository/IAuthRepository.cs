using To_Do_List.API.Models;

namespace To_Do_List.API.Repository
{
    public interface IAuthRepository
    {
        Task<bool> IsUsernameUnique(string username);
        Task<User?> Register(UserDTO user);
        Task<User?> AuthenticateUser(string username, string password);
        Task<User?> GetUser(string id);
        string? CreateToken(User user);
        RefreshToken? GenerateRefreshToken();
        CookieOptions? SetCookieOptionsForToken(RefreshToken refreshToken);
        Task SetRefreshToken(RefreshToken refreshToken, Guid userId);

    }
}
