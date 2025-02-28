using MyStore.Common;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
          Task<bool> IsEmailInUse(string email);
          Task<User?> GetUserWithDetail(LoginDto loginDto);
          Task<List<UserDto>> ReadAllByRole(string role);

    }
}
