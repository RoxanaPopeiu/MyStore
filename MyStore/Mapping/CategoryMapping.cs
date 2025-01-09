using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static class CategoryMapping
    {
        // Map Category to CategoryDto
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                BrandName = category.Brand?.Name,
                Sizes = category.Sizes?.Select(size => size.ToSizeDto()).ToList() ?? new List<SizeDto>()
            };
        }

        // Map a list of Categories to a list of CategoryDtos
        public static List<CategoryDto> ToCategoryDtoList(this IEnumerable<Category> categories)
        {
            return categories.Select(cat => cat.ToCategoryDto()).ToList();
        }
        public static Category ToCategory(this CategoryDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Sizes = categoryDto.Sizes?.Select(size => size.ToSize()).ToList() ?? new List<Size>()
            };
        }

    }
}
