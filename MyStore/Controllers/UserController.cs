using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Models;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            var createUser = await userService.Create(userDto);
            return Ok(createUser);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var extUser =await userService.Login(loginDto);
            return Ok(extUser);
        }
        [HttpPut("Update/{ID:int}")]
        public async Task<UserDto> UpdateProduct(int ID, [FromBody] UserDto userDto)
        {
            return await userService.Update(ID, userDto);
        }
        [HttpDelete("Delete/{ID:int}")]

        public async Task<bool> DeleteProduct(int ID)
        {
            return await userService.Delete(ID);
        }

        [HttpGet("Read/{role}")]
        public async Task<List<UserDto>> ReadUsersByRole(string role)
        {
            return await userService.ReadAllByRole(role);
        }
    }
}
