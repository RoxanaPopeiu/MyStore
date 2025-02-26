using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            var createdProduct = await productService.Create(productDto);
            return Ok(createdProduct);
        }

        [HttpGet("GetAllProducts")]
        public async Task<List<ProductDto>> GetProducts()
        {
            return await productService.ReadAllProducts();
        }
        [HttpGet("GetOneProduct/{ID:int}")]
        public async Task<ProductDto> GetOneProduct(int ID)
        {
            var product = await productService.ReadOneProduct(ID);
            return product?.ToProductDto(); 
        }
        [HttpPut("Update/{ID:int}")]
        public async Task<ProductDto> Update(int ID, ProductDto productDto)
        {
            return await productService.Update(ID, productDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public async Task<bool> Delete(int ID)
        {
            return await productService.Delete(ID);
        }
      
    }
}
