using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static  class UserMapping
    {
        // Map User to UserDto
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Addresses = user.Addresses?.Select(address => address.ToAddressDto()).ToList() ?? new List<AddressDto>()
            };
        }

        // Map a list of Users to a list of UserDtos
        public static List<UserDto> ToUserDtoList(this IEnumerable<User> users)
        {
            return users.Select(user => user.ToUserDto()).ToList();
        }
    }
}
