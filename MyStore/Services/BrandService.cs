using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class BrandService
    {
        private readonly ApplicationDbContext _context;
        public BrandService(ApplicationDbContext context)
        {
            _context = context;
        }
        #region CRUD
        public BrandDto Create(BrandDto brandDto)
        {
            if(CheckBrandExistence(brandDto.Name))
                throw new Exception("The Brand is already registered!"); //to do Custom Exceptions
            var brand = brandDto.ToBrand();
            var result = _context.Brands.Add(brand);
            _context.SaveChanges();
            return result.Entity.ToBrandDto();
        }
        public List<BrandDto> ReadAllBrands()
        {
            var result = BrandMapping.ToBrandDtoList(_context.Brands);    
            return result;
        }
        public Brand ReadOneBrandById(int id)
        {
            return _context.Brands.SingleOrDefault(x => x.Id == id);
        }
        public BrandDto Update(int id, BrandDto brandDto)
        {
            if (CheckBrandExistence(brandDto.Name))
                throw new Exception("The Brand is already registered!"); //to do Custom Exceptions
            var extBrand = ReadOneBrandById(id);
            if(extBrand!=null)
            {
                extBrand.Description = brandDto.Description;
                extBrand.Categories = brandDto.Categories.Select(c => c.ToCategory()).ToList();
                extBrand.Name = brandDto.Name;
                _context.SaveChanges();
                return extBrand.ToBrandDto();
            }
            throw new Exception("The Brand doesn't exist!");
        }
        public bool Delete(int id)
        {
            var extBrand = ReadOneBrandById(id);
            if(extBrand!=null)
            {
                _context.Brands.Remove(extBrand);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion
        #region Private Methods
        private bool CheckBrandExistence(string brandName)
        {
            var result = _context.Brands.FirstOrDefault(x => x.Name == brandName);
            return result != null;
        }
        #endregion
    }
}
