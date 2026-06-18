using SSP.Pure.UsersSync.Contracts;

namespace SSP.Pure.UsersSync.Models;

public class User : ErrorMessage
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
