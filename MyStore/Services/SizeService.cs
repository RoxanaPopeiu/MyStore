using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class SizeService(ApplicationDbContext context): ISizeService
    {
        #region CRUD
        public async Task<SizeDto> Create(SizeDto sizeDto)
        {
            if (await CheckSizeExistence(sizeDto.Name))
                throw new Exception("The Size is already registered!"); //to do Custom Exceptions                                                                      // Create the Size entity
            var size = new Size
            {
                Name = sizeDto.Name,
                Description = sizeDto.Description,
                CategoryId = sizeDto.CategoryId ?? 0
            };
            var result = await context.Sizes.AddAsync(size);
            await context.SaveChangesAsync();
            return result.Entity.ToSizeDto();
        }
        public async Task<List<SizeDto>> ReadAllSizes()
        {
            var sizes = await context.Sizes.ToListAsync();
            return SizeMapping.ToSizeDtoList(sizes);
        }
        public async Task<Size> ReadOneSizeById(int id)
        {
            return await context.Sizes.FindAsync(id);
        }
        public async Task<SizeDto> Update(int id, SizeDto sizeDto)
        {
            var existingSize = await ReadOneSizeById(id); 

            if (existingSize == null)
                throw new Exception("The Size doesn't exist!");

            if (await CheckSizeExistence(sizeDto.Name) && sizeDto.Id != id)
                throw new Exception($"The size '{sizeDto.Name}' is already registered with a different ID!");

            existingSize.Name = sizeDto.Name;
            existingSize.Description = sizeDto.Description;

            await context.SaveChangesAsync();
            return existingSize.ToSizeDto();
        }

        public async Task<bool> Delete(int id)
        {
            var extSize=await ReadOneSizeById(id);
            if (extSize == null)
                return false; 

            context.Sizes.Remove(extSize);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion
        #region Private Methods
        private async Task<bool> CheckSizeExistence(string sizeName)
        {
            return await context.Sizes.AnyAsync(x => x.Name == sizeName);
        }
        #endregion
    }
}
