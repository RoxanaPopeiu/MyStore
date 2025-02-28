using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<bool> IsEmailInUse(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email);

        }
        public async Task<User?> GetUserWithDetail(LoginDto loginDto)
        {
            var user = await context.Users
             .Include(us => us.Addresses)
             .SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            return user;
        }
        public async Task<List<UserDto>> ReadAllByRole(string role)
        {
            var usersWithRole = await context.Users.Where(u => u.Role == role).ToListAsync();
            return UserMapping.ToUserDtoList(usersWithRole);
        }
    }
}
      
