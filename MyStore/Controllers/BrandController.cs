using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController:ControllerBase
    {
        public BrandService brandService { get; set; }
        public BrandController(BrandService brandService)
        {
            this.brandService = brandService;
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] BrandDto brandDto)
        {
            var createBrand = brandService.Create(brandDto);
            return Ok(createBrand);
        }
        [HttpGet("ReadAllBrands")]
        public List<BrandDto> ReadAllBrands()
        {
            return brandService.ReadAllBrands();
        }
        [HttpGet("ReadOneBrand/{ID:int}")]
        public BrandDto ReadOneBrandById(int ID)
        {
            return brandService.ReadOneBrandById(ID).ToBrandDto();
        }
        [HttpPut("Update/{ID:int}")]
        public BrandDto Update(int ID, [FromBody] BrandDto brandDto)
        {
            return brandService.Update(ID, brandDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public bool Delete(int ID)
        {
            return brandService.Delete(ID);
        }


    }
}
