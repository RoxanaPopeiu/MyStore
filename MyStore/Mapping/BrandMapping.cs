using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static class BrandMapping
    {
        public static BrandDto ToBrandDto(this Brand brand)
        {
            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Categories = brand.Categories?.Select(c => c.ToCategoryDto()).ToList() ?? new List<CategoryDto>()
            };
        }

        // Map a list of Brands to a list of BrandDtos
        public static List<BrandDto> ToBrandDtoList(this IEnumerable<Brand> brands)
        {
            return brands.Select(brand => brand.ToBrandDto()).ToList();
        }

    }
}
