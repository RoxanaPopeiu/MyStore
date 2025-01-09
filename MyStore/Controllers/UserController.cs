using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Models;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserService userService { get; set; }
        public UserController(UserService userService)
        {
            this.userService = userService;
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserDto userDto)
        {

            var createUser = userService.Create(userDto);
            return Ok(createUser);
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {

            var extUser = userService.Login(loginDto);
            return Ok(extUser);
        }
        [HttpPut("Update/{ID:int}")]
        public UserDto UpdateProduct(int ID, [FromBody] UserDto userDto)
        {
            return userService.Update(ID, userDto);

        }
        [HttpDelete("Delete/{ID:int}")]

        public bool DeleteProduct(int ID)
        {
            return userService.Delete(ID);

        }

        [HttpGet("Read/{role}")]
        public List<UserDto> ReadUsersByRole(string role)
        {
            return userService.ReadAllByRole(role);
        }
    }
}
