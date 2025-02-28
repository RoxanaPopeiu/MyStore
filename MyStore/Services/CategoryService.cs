using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class CategoryService(ICategoryRepository categoryRepository
        , IBrandRepository brandRepository
        , ISizeRepository sizeRepository) :ICategoryService
    {
        #region CRUD
        public async Task<CategoryDto> Create(CategoryDto categoryDto)
        {
            if (await categoryRepository.ExistsByNameAsync(categoryDto.Name))
                throw new Exception("The Category already exists!");      

            var existingBrand = await brandRepository.GetByIdAsync(categoryDto.BrandId);
            if (existingBrand == null)
                throw new Exception("Brand does not exist. Please create the brand first.");

            var category = categoryDto.ToCategory();
            category.Brand = existingBrand;

            var result = await categoryRepository.AddAsync(category);
            return result.ToCategoryDto();

        }
        public async Task<List<CategoryDto>> ReadAllCategory()
        {
            var categoryList = await categoryRepository.GetAllAsync();  
            return categoryList.ToCategoryDtoList();

        }
        public async Task<CategoryDto> ReadOneCategoryById(int id)
        {
            var category=await categoryRepository.GetByIdAsync(id);
            return category?.ToCategoryDto() ;
        }

        public async Task<CategoryDto> Update(int id, CategoryDto categoryDto)
        {
            var extCategory = await categoryRepository.GetByIdAsync(id);
            if (extCategory == null)
                throw new Exception("Category not found.");

            var existingBrand = await brandRepository.GetByIdAsync(categoryDto.BrandId);
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
                    var existingSize = await sizeRepository.GetByIdAsync(sizeDto.Id);
                    if (existingSize != null)
                    {
                        existingSize.Name = sizeDto.Name;
                        existingSize.Description = sizeDto.Description;
                        await sizeRepository.UpdateAsync(existingSize); 

                    }
                    else
                    {
                        if (extCategory.Sizes == null) 
                            extCategory.Sizes = new List<Size>();

                        extCategory.Sizes.Add(new Size
                        {
                            Name = sizeDto.Name,
                            Description = sizeDto.Description,
                            CategoryId = id
                        });
                    }
                }
            }

            await categoryRepository.UpdateAsync(extCategory);
            return extCategory.ToCategoryDto();
        }

        public async Task<bool> Delete(int id)
        {
            var extCategory = await categoryRepository.GetByIdAsync(id);
            if (extCategory == null)
            {
                return false;
            }
            await categoryRepository.DeleteAsync(extCategory);
            return true;
        }

        #endregion

    }
}
