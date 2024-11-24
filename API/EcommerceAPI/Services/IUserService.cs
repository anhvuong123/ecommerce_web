using EcommerceAPI.Models;
using EcommerceLib.Dtos;

namespace EcommerceAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName);
    }
}
