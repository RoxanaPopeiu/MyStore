using System.ComponentModel.DataAnnotations;

namespace MyStore.DTO
{
    public class UserDto
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email {  get; set; }
        public string Role {  get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }
        public List<AddressDto>? Addresses { get; set; } = new List<AddressDto>();
    }
}
