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
                CategoryId=product.CategoryId,
                CategoryName = product.Category?.Name,
                BrandId=product.BrandId,
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
        public static Product ToProduct(this ProductDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                BrandId=productDto.BrandId,
                CategoryId=productDto.CategoryId,
                PromotionId=productDto.PromotionId,
                ProductSizes=productDto.Sizes.Select(sizeDto => new ProductSize
                {               
                    Size = sizeDto.ToSize(), 
                }).ToList()
            };
        }
    }
}
