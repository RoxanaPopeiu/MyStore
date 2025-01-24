using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        public ProductService productService {  get; set; }
        public ProductController (ProductService productService)
        {
            this.productService = productService;
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] ProductDto productDto)
        {
            return Ok(productService.Create(productDto));
        }
        [HttpGet("GetAllProducts")]
        public List<ProductDto> GetProducts()
        {
            return productService.ReadAllProducts();
        }
        [HttpGet("GetOneProduct/{ID:int}")]
        public ProductDto GetOneProduct(int ID)
        {
            return productService.ReadOneProduct(ID).ToProductDto();
        }
        [HttpPut("Update/{ID:int}")]
        public ProductDto Update(int ID, ProductDto productDto)
        {
            return productService.Update(ID, productDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public bool Delete(int ID)
        {
            return productService.Delete(ID);
        }
      
    }
}
