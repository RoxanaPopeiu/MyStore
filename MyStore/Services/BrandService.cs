using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class BrandService(ApplicationDbContext context): IBrandService
    {
        #region CRUD
        public async Task<BrandDto> Create(BrandDto brandDto)
        {
            if(CheckBrandExistence(brandDto.Name))
                throw new Exception("The Brand is already registered!"); //to do Custom Exceptions
            var brand = brandDto.ToBrand();
            var result =await context.Brands.AddAsync(brand);
            await context.SaveChangesAsync();
            return result.Entity.ToBrandDto();
        }
        public async Task<List<BrandDto>> ReadAllBrands()
        {
            var brands = await context.Brands.ToListAsync();
            return BrandMapping.ToBrandDtoList(brands);
        }
        public async Task<Brand?> ReadOneBrandById(int id)
        {
            return await context.Brands.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<BrandDto> Update(int id, BrandDto brandDto)
        {
            if (CheckBrandExistence(brandDto.Name))
                throw new Exception("The Brand is already registered!"); //to do Custom Exceptions
            var extBrand =await ReadOneBrandById(id);
            if(extBrand!=null)
            {
                extBrand.Description = brandDto.Description;
                extBrand.Categories = brandDto.Categories.Select(c => c.ToCategory()).ToList();
                extBrand.Name = brandDto.Name;
                await context.SaveChangesAsync();
                return extBrand.ToBrandDto();
            }
            throw new Exception("The Brand doesn't exist!");
        }
        public async Task<bool> Delete(int id)
        {
            var extBrand = await ReadOneBrandById(id);
            if(extBrand!=null)
            {
                context.Brands.Remove(extBrand);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
        #region Private Methods
        private bool CheckBrandExistence(string brandName)
        {
            return context.Brands.Any(x => x.Name == brandName);

        }
        #endregion
    }
}
