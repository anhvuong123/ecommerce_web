using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceLib.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class UserService : IUserService
    {
        private readonly EcommerceContext _context;

        public UserService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _context.Users
                .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(_context.Roles, uur => uur.ur.RoleId, r => r.Id, (uur, r) => new { uur.u, r })
                .Where(x => x.r.Name == roleName)
                .Select(x => new UserDto
                {
                    UserName = x.u.UserName,
                    Email = x.u.Email,
                    PhoneNumber = x.u.PhoneNumber,
                    RoleName = x.r.Name
                })
                .ToListAsync();

            return users;
        }

    }
}
