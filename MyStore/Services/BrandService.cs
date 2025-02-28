using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class BrandService(IBrandRepository brandRepository) : IBrandService
    {
        #region CRUD
        public async Task<BrandDto> Create(BrandDto brandDto)
        {
            if(await brandRepository.ExistsByNameAsync(brandDto.Name))
                throw new Exception("The Brand is already registered!"); //to do Custom Exceptions

            var brand = brandDto.ToBrand();
            var result =await brandRepository.AddAsync(brand);
            return result.ToBrandDto();
        }
        public async Task<List<BrandDto>> ReadAllBrands()
        {
            var brands = await brandRepository.GetAllAsync();
            return brands.ToBrandDtoList();
        }
        public async Task<BrandDto?> ReadOneBrandById(int id)
        {
           var brand=await brandRepository.GetByIdAsync(id);
            return brand.ToBrandDto() ;
        }
        public async Task<BrandDto> Update(int id, BrandDto brandDto)
        {
            var extBrand = await brandRepository.GetByIdAsync(id);
            if (extBrand == null)
                throw new Exception("The Brand doesn't exist!");

            if (extBrand.Name != brandDto.Name && await brandRepository.ExistsByNameAsync(brandDto.Name))
                throw new Exception("The Brand is already registered!");

            extBrand.Description = brandDto.Description;
            extBrand.Categories = brandDto.Categories.Select(c => c.ToCategory()).ToList();
            extBrand.Name = brandDto.Name;

            await brandRepository.UpdateAsync(extBrand);
            return extBrand.ToBrandDto();

        }
        public async Task<bool> Delete(int id)
        {
            var extBrand = await brandRepository.GetByIdAsync(id);
            if (extBrand==null)
            {
                return false;
            }
            await brandRepository.DeleteAsync(extBrand);
            return true;

        }
        #endregion

    }
}
