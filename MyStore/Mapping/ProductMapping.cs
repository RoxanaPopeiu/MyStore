using MyStore.DTO;
using MyStore.Models;
using System.Xml.Linq;

namespace MyStore.Mapping
{
    public static class ProductMapping
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category?.Name,
                BrandName = product.Brand?.Name,
                PromotionName = product.Promotion?.Value.ToString(),
                Sizes = product.ProductSizes.Select(ps => ps.Size.ToSizeDto()).ToList()
            };
        }
        // Map a list of Products to a list of ProductsDto
        public static List<ProductDto> ToProductDtoList(this IEnumerable<Product> products)
        {
            return products.Select(product => product.ToProductDto()).ToList();
        }
        public static Product ToProduct(this ProductDto productDto, CategoryDto categoryDto, BrandDto brandDto, PromotionDto promotionDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category=categoryDto.ToCategory(),
                Brand=brandDto.ToBrand(),
                Promotion=promotionDto.ToPromotion(),
                ProductSizes=productDto.Sizes.Select(sizeDto => new ProductSize
                {
                    // Map the necessary properties from SizeDto to ProductSize
                    Size = sizeDto.ToSize(), // Assuming `ToSize()` maps SizeDto to Size
                                             // Add other mappings if ProductSize has additional properties
                }).ToList()
            };
        }
    }
}
