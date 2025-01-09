using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        #region CRUD
        public UserDto Create(UserDto userDto)
        {

            if (IsEmailInUse(userDto.Email))
                throw new Exception("The Email is already used!"); //to do Custom Exceptions
            var user = userDto.ToUser();
            var result =_context.Users.Add(user);
            _context.SaveChanges();
            return result.Entity.ToUserDto();
        }
        //Read
        public UserDto Login(LoginDto loginDto)
        {
            if(!IsEmailInUse(loginDto.Email))
                throw new Exception("Wrong credentials!"); //to do Custom Exceptions
            var user=_context.Users
                .Include(us => us.Addresses)
                .SingleOrDefault(u=>u.Email == loginDto.Email && u.Password==loginDto.Password);
            if (user == null)
                throw new Exception("Wrong credentials!");
            return user.ToUserDto() ;
        }
        public UserDto Update(int Id,UserDto userDto)
        {
            var extUser = GetUserById(Id);
            if (extUser != null)
            {
                extUser.UserName = userDto.UserName;
                extUser.Addresses = userDto.Addresses.Select(a => a.ToAdress()).ToList();
                extUser.Password = userDto.Password;
                extUser.Role = userDto.Role;
                _context.SaveChanges();
                return extUser.ToUserDto();
            }
            throw new Exception("The User doesn't exist!");

        }
        public bool Delete(int id)
        {
            var extUser= GetUserById(id);
            if(extUser != null)
            {
                _context.Users.Remove(extUser);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<UserDto> ReadAllByRole(string role)
        {
            var usersWithRole = _context.Users.Where(u => u.Role == role).ToList();
            return UserMapping.ToUserDtoList(usersWithRole);
        }

        #endregion
        #region Private Methods
        private bool IsEmailInUse(string email)
        {
            var result=_context.Users.FirstOrDefault(x => x.Email == email);
            return result != null;
        }
        private User GetUserById(int Id)
        {
            return _context.Users.SingleOrDefault(x=>x.Id == Id);
        }
        #endregion

    }
}
