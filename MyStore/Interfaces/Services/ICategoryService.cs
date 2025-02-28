using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Interfaces.Services
{
    public interface ICategoryService
    {

        public Task<CategoryDto> Create(CategoryDto categoryDto);
        public Task<List<CategoryDto>> ReadAllCategory();
        public Task<CategoryDto> ReadOneCategoryById(int id);
        public Task<CategoryDto> Update(int id, CategoryDto categoryDto);
        public Task<bool> Delete(int id);

    }
}
