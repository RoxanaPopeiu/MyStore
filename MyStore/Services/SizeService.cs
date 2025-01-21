using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class SizeService
    {
        private readonly ApplicationDbContext _context;
        public SizeService(ApplicationDbContext context)
        {
            _context = context;
        }
        #region CRUD
        public SizeDto Create(SizeDto sizeDto)
        {
            if(CheckSizeExistince(sizeDto.Name))
                throw new Exception("The Size is already registered!"); //to do Custom Exceptions                                                                      // Create the Size entity
            var size = new Size
            {
                Name = sizeDto.Name,
                Description = sizeDto.Description,
                CategoryId = sizeDto.CategoryId ?? 0
            };
            var result=_context.Sizes.Add(size);
            _context.SaveChanges();
            return result.Entity.ToSizeDto();
        }
        public List<SizeDto> ReadAllSizes()
        {
            var result = SizeMapping.ToSizeDtoList(_context.Sizes);
            return result;
        }
        public Size ReadOneSizeById(int id)
        {
            return _context.Sizes.SingleOrDefault(s => s.Id == id);
        }
        public SizeDto Update(int id, SizeDto sizeDto)
        {
            if (CheckSizeExistince(sizeDto.Name) && sizeDto.Id != id)
                throw new Exception("The Size is already registered!"); //to do Custom Exceptions
            var existingSize = ReadOneSizeById(id);
            if (existingSize!=null)
            {
                existingSize.Name=sizeDto.Name;
                existingSize.Description=sizeDto.Description;
                _context.SaveChanges();
                return existingSize.ToSizeDto() ;

            }
            throw new Exception("The Size doesn't exist!");
        }
        public bool Delete(int id)
        {
            var extSize=ReadOneSizeById(id);
            if(extSize!=null)
            {
                _context.Sizes.Remove(extSize);
                _context.SaveChanges() ;
                return true;
            }
            return false;
        }
        #endregion
        # region Private Methods
        private bool CheckSizeExistince(string sizeName)
        {
            var result=_context.Sizes.FirstOrDefault(x => x.Name == sizeName);
            return result!=null;
        }

        #endregion
    }
}
