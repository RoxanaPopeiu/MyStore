using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;
using static MyStore.Services.ProductService;

namespace MyStore.Interfaces.Services
{
    public interface IProductService
    {
        public Task<ProductDto> Create(ProductDto productDto);
        public Task<List<ProductDto>> ReadAllProducts();
        public Task<Product> ReadOneProduct(int id);
        public Task<ProductDto> Update(int id, ProductDto productDto);
        public Task<bool> Delete(int id);

    }
}
