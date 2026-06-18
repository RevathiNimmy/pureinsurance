namespace SSP.Pure.UsersSync.Contracts;

public sealed record AuthResponseDTO(UserResponseDTO User, string AccessToken, string RefreshToken, ErrorMessage Error);