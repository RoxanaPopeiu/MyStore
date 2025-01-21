using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController:ControllerBase
    {
        public CategoryService categoryServices { get; set; }
        public CategoryController(CategoryService categoryServices)
        {
            this.categoryServices = categoryServices;
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryDto categoryDto)
        {
            var createCategory=categoryServices.Create(categoryDto);
            return Ok(createCategory);
        }
        [HttpGet("ReadAllCategories")]
        public List<CategoryDto> ReadAllCategories()
        {
            return categoryServices.ReadAllCategory();
        }
        [HttpGet("ReadOneCategoryById/{ID:int}")]
        public CategoryDto ReadOneCategoryById(int ID)
        {
            return categoryServices.ReadOneCategoryById(ID).ToCategoryDto();
        }
        [HttpPut("Update/{ID:int}")]
        public CategoryDto Update(int ID, [FromBody] CategoryDto categoryDto)
        {
            return categoryServices.Update(ID, categoryDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public bool Delete(int ID)
        {
            return categoryServices.Delete(ID);
        }
    }
    
}
