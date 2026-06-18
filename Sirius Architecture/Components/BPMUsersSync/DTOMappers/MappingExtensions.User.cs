using KeycloackTest.Contracts;
using KeycloackTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace KeycloackTest.DTOMappers;

public static class MappingExtensions
{
    public static UserResponseDTO ToDTO(this User user)
    {
        return new UserResponseDTO(user.Id, user.UserName, user.Email);
    }

    public static IReadOnlyList<UserResponseDTO> ToDTO(this IEnumerable<User> users)
    {
        return users.Select(user => user.ToDTO()).ToList();
    }
}
