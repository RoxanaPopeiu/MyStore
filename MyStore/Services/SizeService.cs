using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class SizeService(ISizeRepository sizeRepository) : ISizeService
    {
        #region CRUD
        public async Task<SizeDto> Create(SizeDto sizeDto)
        {
            if (await sizeRepository.CheckSizeExistence(sizeDto.Name))
                throw new Exception("The Size is already registered!");                                                           
            var size = new Size
            {
                Name = sizeDto.Name,
                Description = sizeDto.Description,
                CategoryId = sizeDto.CategoryId ?? 0
            };
            var result = await sizeRepository.AddAsync(size);
            return result.ToSizeDto();
        }
        public async Task<List<SizeDto>> ReadAllSizes()
        {
            var sizes = await sizeRepository.GetAllAsync();
            return sizes.ToSizeDtoList();
        }
        public async Task<SizeDto> ReadOneSizeById(int id)
        {
            var size=await sizeRepository.GetByIdAsync(id);
            return size?.ToSizeDto();
        }
        public async Task<SizeDto> Update(int id, SizeDto sizeDto)
        {
            var existingSize = await sizeRepository.GetByIdAsync(id);
            if (existingSize == null)
                throw new Exception("The Size doesn't exist!");

            if (existingSize.Name != sizeDto.Name && await sizeRepository.CheckSizeExistence(sizeDto.Name))
                throw new Exception($"The size '{sizeDto.Name}' is already registered!");


            existingSize.Name = sizeDto.Name;
            existingSize.Description = sizeDto.Description;

            await sizeRepository.UpdateAsync(existingSize);
            return existingSize.ToSizeDto();
        }

        public async Task<bool> Delete(int id)
        {
            var extSize= await sizeRepository.GetByIdAsync(id);
            if (extSize == null)
                return false;

            await sizeRepository.DeleteAsync(extSize);
            return true;
        }
        #endregion

    }
}
