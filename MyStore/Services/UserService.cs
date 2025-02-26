using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Services
{
    public class UserService(ApplicationDbContext context): IUserService
    {
        #region CRUD
        public async Task<UserDto> Create(UserDto userDto)
        {

            if (await IsEmailInUse(userDto.Email))
                throw new Exception("The Email is already used!"); //to do Custom Exceptions
            var user = userDto.ToUser();
            var result =await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return result.Entity.ToUserDto();
        }
        public async Task<UserDto> Login(LoginDto loginDto)
        {
            if(!await IsEmailInUse(loginDto.Email))
                throw new Exception("Wrong credentials!"); //to do Custom Exceptions
            var user=await context.Users
                .Include(us => us.Addresses)
                .SingleOrDefaultAsync(u=>u.Email == loginDto.Email && u.Password==loginDto.Password);
            if (user == null)
                throw new Exception("Wrong credentials!");
            return user.ToUserDto() ;
        }
        public async Task<UserDto> Update(int Id,UserDto userDto)
        {
            var extUser =await GetUserById(Id);
            if (extUser != null)
            {
                extUser.UserName = userDto.UserName;
                extUser.Addresses = userDto.Addresses.Select(a => a.ToAdress()).ToList();
                extUser.Password = userDto.Password;
                extUser.Role = userDto.Role;
                await context.SaveChangesAsync();
                return extUser.ToUserDto();
            }
            throw new Exception("The User doesn't exist!");

        }
        public async Task<bool> Delete(int id)
        {
            var extUser= await GetUserById(id);
            if(extUser != null)
            {
                context.Users.Remove(extUser);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<UserDto>> ReadAllByRole(string role)
        {
            var usersWithRole = await context.Users.Where(u => u.Role == role).ToListAsync();
            return UserMapping.ToUserDtoList(usersWithRole);
        }

        #endregion
        #region Private Methods
        private async Task<bool> IsEmailInUse(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email);
       
        }
        private async Task<User> GetUserById(int Id)
        {
            return await context.Users.SingleOrDefaultAsync(x=>x.Id == Id);
        }
        #endregion

    }
}
