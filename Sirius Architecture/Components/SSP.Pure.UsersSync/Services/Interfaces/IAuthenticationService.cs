using SSP.Pure.UsersSync.Contracts;
using SSP.Pure.UsersSync.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSP.Pure.UsersSync.Services;

public interface IAuthenticationService
{
    Task<IReadOnlyList<UserResponseDTO>> GetAllUsersAsync();
    Task<AuthResponseDTO> RegisterUserAsync(UserRegisterRequestDTO requestDTO);
    Task<AuthResponseDTO> LoginUserAsync(UserLoginRequestDTO request);
    Task<string> GetUserAsync(string userName);
    Task<AuthResponseDTO> UpdateUserAsync(UserRegisterRequestDTO request);
    //KeyCloakConfiguration GetKeyCloakConfiguration(string userName, string password, int userKey);
    Task<KeycloakToken> GetAdminTokenAsync();
}
