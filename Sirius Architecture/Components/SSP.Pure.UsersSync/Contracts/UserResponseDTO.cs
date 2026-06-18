namespace SSP.Pure.UsersSync.Contracts;

public sealed record UserResponseDTO(string Id, string UserName, string Email, ErrorMessage Error);
