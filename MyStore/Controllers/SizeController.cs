using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SizeController : ControllerBase
    {
        public SizeService sizeService { get; set; }
        public SizeController(SizeService sizeService)
        {
            this.sizeService = sizeService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] SizeDto sizeDto)
        {
            var createSize = sizeService.Create(sizeDto);
            return Ok(createSize);
        }
        [HttpGet("ReadAllSizes")]
        public List<SizeDto> ReadAllSizes()
        {
            return sizeService.ReadAllSizes();
        }
        [HttpGet("ReadOneSize/{ID:int}")]
        public SizeDto ReadOneSizeById(int ID)
        {
            return sizeService.ReadOneSizeById(ID).ToSizeDto();
        }
        [HttpPut("Update/{ID:int}")]
        public SizeDto Update(int ID, [FromBody] SizeDto sizeDto)
        {
            return sizeService.Update(ID, sizeDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public bool Delete(int ID)
        {
            return sizeService.Delete(ID);
        }
    }
}
