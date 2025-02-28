using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Services
{
    public class UserService(IUserRepository userRepository): IUserService
    {
        #region CRUD
        public async Task<UserDto> Create(UserDto userDto)
        {

            if (await userRepository.IsEmailInUse(userDto.Email))
                throw new Exception("The Email is already used!");
            var user = userDto.ToUser();
            var result =await userRepository.AddAsync(user);     
            return result.ToUserDto();
        }
        public async Task<UserDto> Login(LoginDto loginDto)
        {
            if(!await userRepository.IsEmailInUse(loginDto.Email))
                throw new Exception("Wrong credentials!");
            var user = await userRepository.GetUserWithDetail(loginDto);
            if (user == null)
                throw new Exception("Wrong credentials!");
            return user.ToUserDto();
        }
        public async Task<UserDto> Update(int id,UserDto userDto)
        {
            var extUser =await userRepository.GetByIdAsync(id);
            if (extUser != null)
            {
                extUser.UserName = userDto.UserName;
                extUser.Addresses = userDto.Addresses.Select(a => a.ToAdress()).ToList();
                extUser.Password = userDto.Password;
                extUser.Role = userDto.Role;
                await userRepository.UpdateAsync(extUser);
                return extUser.ToUserDto();
            }
            throw new Exception("The User doesn't exist!");

        }
        public async Task<bool> Delete(int id)
        {
            var extUser= await userRepository.GetByIdAsync(id);
            if (extUser != null)
            {
                await userRepository.DeleteAsync(extUser);
                return true;
            }
            return false;
        }

        public async Task<List<UserDto>> ReadAllByRole(string role)
        {
            var usersWithRole = await userRepository.ReadAllByRole(role);
            return usersWithRole;
        }

        #endregion


    }
}
