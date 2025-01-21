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
            // Convert DTO to Entity
            var category =categoryDto.ToCategory();
            var result=_context.Categories.Add(category);
            _context.SaveChanges();
            return result.Entity.ToCategoryDto();

        }
        public List<CategoryDto> ReadAllCategory()
        {
            var result=CategoryMapping.ToCategoryDtoList(_context.Categories);
            return result;

        }
        public Category ReadOneCategoryById(int id)
        {
            return _context.Categories.SingleOrDefault(x =>x.Id == id);
        }
        public CategoryDto Update(int id, CategoryDto categoryDto)
        {
            if (CheckCategoryExistence(categoryDto.Name))
                throw new Exception("The Category already exists!"); //to do Custom Exceptions
            var extCategory=ReadOneCategoryById(id);
            if(extCategory != null)
            {
                extCategory.Name = categoryDto.Name;
                extCategory.Description = categoryDto.Description;
                extCategory.Brand= _context.Brands.FirstOrDefault(b => b.Name == categoryDto.BrandName);
                // Update Sizes if provided
                if (categoryDto.Sizes != null && categoryDto.Sizes.Any())
                {
                    foreach (var sizeDto in categoryDto.Sizes)
                    {
                        var existingSize = _context.Sizes.FirstOrDefault(s => s.Id == sizeDto.Id);
                        if (existingSize != null)
                        {
                            // Update existing size
                            existingSize.Name = sizeDto.Name;
                            existingSize.Description = sizeDto.Description;
                            existingSize.CategoryId = id; // Link the size to this category
                        }
                        else
                        {
                            // Add new size
                            var newSize = new Size
                            {
                                Name = sizeDto.Name,
                                Description = sizeDto.Description,
                                CategoryId = id
                            };
                            _context.Sizes.Add(newSize);
                        }
                    }
                }
                _context.SaveChanges();
            }
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
