using SSP.Pure.UsersSync.Contracts;
using SSP.Pure.UsersSync.Models;
using System.Collections.Generic;
using System.Linq;

namespace SSP.Pure.UsersSync.DTOMappers;

public static class MappingExtensions
{
    public static UserResponseDTO ToDTO(this User user)
    {
        ErrorMessage error = new ErrorMessage() {ErrorDetails =user.ErrorDetails,ErrorResponseCode=user.ErrorResponseCode };
        return new UserResponseDTO(user.Id, user.UserName, user.Email,error);
    }

    public static IReadOnlyList<UserResponseDTO> ToDTO(this IEnumerable<User> users)
    {
        return users.Select(user => user.ToDTO()).ToList();
    }
}
