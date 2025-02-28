using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Interfaces.Services
{
    public interface IBrandService
    {
        public Task<BrandDto> Create(BrandDto brandDto);
        public Task<List<BrandDto>> ReadAllBrands();
        public Task<BrandDto?> ReadOneBrandById(int id);
        public Task<BrandDto> Update(int id, BrandDto brandDto);
        public Task<bool> Delete(int id);
    }
}
