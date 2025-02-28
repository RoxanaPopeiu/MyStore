using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ICategoryService categoryServices) :ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            var createCategory=await categoryServices.Create(categoryDto);
            return Ok(createCategory);
        }
        [HttpGet("ReadAllCategories")]
        public async Task<List<CategoryDto>> ReadAllCategories()
        {
            return await categoryServices.ReadAllCategory();
        }
        [HttpGet("ReadOneCategoryById/{ID:int}")]
        public async Task<CategoryDto> ReadOneCategoryById(int ID)
        {
            var category = await categoryServices.ReadOneCategoryById(ID);
            return category;
        }
        [HttpPut("Update/{ID:int}")]
        public async Task<CategoryDto> Update(int ID, [FromBody] CategoryDto categoryDto)
        {
            return await categoryServices.Update(ID, categoryDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public async Task<bool> Delete(int ID)
        {
            return await categoryServices.Delete(ID);
        }
    }
    
}
