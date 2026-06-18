namespace SSP.Pure.UsersSync.Contracts;

public sealed record UserRegisterRequestDTO(string UserName, string Email, string Password,string AdminGroupName,string Id,string FirstName,string LastName,int Deleted);
