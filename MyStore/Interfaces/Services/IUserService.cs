using MyStore.DTO;
using MyStore.Mapping;

namespace MyStore.Interfaces.Services
{
    public interface IUserService
    {
        public Task<UserDto> Create(UserDto userDto);
        public Task<UserDto> Login(LoginDto loginDto);
        public Task<UserDto> Update(int Id, UserDto userDto);
        public Task<bool> Delete(int id);
        public Task<List<UserDto>> ReadAllByRole(string role);
     

    }
}
