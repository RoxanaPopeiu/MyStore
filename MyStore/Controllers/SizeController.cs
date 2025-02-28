using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SizeController(ISizeService sizeService) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SizeDto sizeDto)
        {
            var createSize = await sizeService.Create(sizeDto);
            return Ok(createSize);
        }
        [HttpGet("ReadAllSizes")]
        public async Task<List<SizeDto>> ReadAllSizes()
        {
            return await sizeService.ReadAllSizes();
        }
        [HttpGet("ReadOneSize/{ID:int}")]
        public async Task<SizeDto> ReadOneSizeById(int ID)
        {
            var size = await sizeService.ReadOneSizeById(ID);
            return size;
        }
        [HttpPut("Update/{ID:int}")]
        public async Task<SizeDto> Update(int ID, [FromBody] SizeDto sizeDto)
        {
            return await sizeService.Update(ID, sizeDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public async Task<bool> Delete(int ID)
        {
            return await sizeService.Delete(ID);
        }
    }
}
