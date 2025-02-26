using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class CategoryService(ApplicationDbContext context):ICategoryService
    {
        #region CRUD
        public async Task<CategoryDto> Create(CategoryDto categoryDto)
        {
            if (await CheckCategoryExistence(categoryDto.Name))
                throw new Exception("The Category already exists!"); //to do Custom Exceptions
                                                                     // Fetch the existing brand before mapping
            var existingBrand = await context.Brands.FirstOrDefaultAsync(b => b.Id == categoryDto.BrandId || b.Name == categoryDto.BrandName);

            if (existingBrand == null)
                throw new Exception("Brand does not exist. Please create the brand first.");
            // Convert DTO to Entity
            var category = categoryDto.ToCategory();
            category.Brand = existingBrand;
            var result = await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return result.Entity.ToCategoryDto();

        }
        public async Task<List<CategoryDto>> ReadAllCategory()
        {
            var result = CategoryMapping.ToCategoryDtoList(
                context.Categories
                    .Include(c => c.Brand)
                    .Include(c => c.Sizes)
            );
            return result;

        }
        public async Task<Category> ReadOneCategoryById(int id)
        {
            return await context.Categories
                .Include(c => c.Brand)
                .Include(c => c.Sizes)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CategoryDto> Update(int id, CategoryDto categoryDto)
        {
            var extCategory = await ReadOneCategoryById(id);
            if (extCategory == null)
                throw new Exception("Category not found.");

            var existingBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == categoryDto.BrandName);
            if (existingBrand == null)
                throw new Exception("Brand does not exist. Please create the brand first.");

            extCategory.BrandId = existingBrand.Id;
            extCategory.Brand = existingBrand;
            extCategory.Name = categoryDto.Name;
            extCategory.Description = categoryDto.Description;

            if (categoryDto.Sizes != null && categoryDto.Sizes.Any())
            {
                foreach (var sizeDto in categoryDto.Sizes)
                {
                    var existingSize = await context.Sizes.FirstOrDefaultAsync(s => s.Id == sizeDto.Id);
                    if (existingSize != null)
                    {
                        existingSize.Name = sizeDto.Name;
                        existingSize.Description = sizeDto.Description;
                    }
                    else
                    {
                        extCategory.Sizes.Add(new Size
                        {
                            Name = sizeDto.Name,
                            Description = sizeDto.Description,
                            CategoryId = id
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
            return extCategory.ToCategoryDto();
        }


        public async Task<bool> Delete(int id)
        {
            var extCategory = await ReadOneCategoryById(id);
            if (extCategory != null)
            {
                context.Categories.Remove(extCategory);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        #endregion
        #region Private Methods
        private async Task<bool> CheckCategoryExistence(string categoryName)
        {
            return await context.Categories.AnyAsync(x => x.Name == categoryName);
        }

        #endregion
    }
}
