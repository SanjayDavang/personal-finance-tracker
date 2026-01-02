using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Infrastructure.Data;
using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.Interfaces;

namespace PersonalFinanceTracker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.UserName == username);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AssignRoleAsync(int userId, string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (role == null)
            {
                throw new Exception($"Role {roleName} Not Found");
            }

            var alreadyAssign = await _context.UserRoles.AnyAsync(ur => ur.User_Id == userId && ur.Role_Id == role.Role_Id);

            if (alreadyAssign) return;

            var userRole = new UserRole
            {
                User_Id = userId,
                Role_Id = role.Role_Id
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

        }
    }
}
