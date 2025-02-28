using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController(IBrandService brandService):ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BrandDto brandDto)
        {
            var createBrand =await brandService.Create(brandDto);
            return Ok(createBrand);
        }
        [HttpGet("ReadAllBrands")]
        public async Task<List<BrandDto>> ReadAllBrands()
        {
            return await brandService.ReadAllBrands();
        }
        [HttpGet("ReadOneBrand/{ID:int}")]
        public async Task<BrandDto> ReadOneBrandById(int ID)
        {
            var brand = await brandService.ReadOneBrandById(ID); 
            return brand; 
        }
        [HttpPut("Update/{ID:int}")]
        public async Task<BrandDto> Update(int ID, [FromBody] BrandDto brandDto)
        {
            return await brandService.Update(ID, brandDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public async Task<bool> Delete(int ID)
        {
            return await brandService.Delete(ID);
        }


    }
}
