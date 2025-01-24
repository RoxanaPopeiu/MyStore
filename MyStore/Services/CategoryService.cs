using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService (ApplicationDbContext context)
        {
            _context = context;
        }
        #region CRUD
        public CategoryDto Create(CategoryDto categoryDto)
        { 
            if(CheckCategoryExistence(categoryDto.Name))
                throw new Exception("The Category already exists!"); //to do Custom Exceptions
             // Fetch the existing brand before mapping
            var existingBrand = _context.Brands.FirstOrDefault(b => b.Id == categoryDto.BrandId || b.Name==categoryDto.BrandName);

            if (existingBrand == null)
                throw new Exception("Brand does not exist. Please create the brand first.");
            // Convert DTO to Entity
            var category =categoryDto.ToCategory();
            category.Brand = existingBrand;
            var result=_context.Categories.Add(category);
            _context.SaveChanges();
            return result.Entity.ToCategoryDto();

        }
        public List<CategoryDto> ReadAllCategory()
        {
            var result = CategoryMapping.ToCategoryDtoList(
                _context.Categories
                    .Include(c => c.Brand)  
                    .Include(c => c.Sizes)  
            );
            return result;

        }
        public Category ReadOneCategoryById(int id)
        {
            return _context.Categories
                    .Include(c => c.Brand)  
                    .Include(c => c.Sizes)  
                    .SingleOrDefault(x => x.Id == id);
        }
        public CategoryDto Update(int id, CategoryDto categoryDto)
        {

            var extCategory=ReadOneCategoryById(id);
            if (extCategory == null)
                throw new Exception("Category not found.");
            var existingBrand = _context.Brands.FirstOrDefault(b => b.Name == categoryDto.BrandName);
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
                        var existingSize = _context.Sizes.FirstOrDefault(s => s.Id == sizeDto.Id);
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

            _context.SaveChanges();
            return extCategory.ToCategoryDto();

        }
        public bool Delete(int id)
        {
            var extCategory = ReadOneCategoryById(id);
            if (extCategory != null)
            {
                _context.Categories.Remove(extCategory);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        #endregion
        #region Private Methods
        private bool CheckCategoryExistence(string categoryName)
        {
            var result=_context.Categories.FirstOrDefault(x=>x.Name == categoryName);
            return result != null;
        }
        #endregion
    }
}
