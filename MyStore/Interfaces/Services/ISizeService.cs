using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Interfaces.Services
{
    public interface ISizeService
    {
        public Task<SizeDto> Create(SizeDto sizeDto);
        public Task<List<SizeDto>> ReadAllSizes();
        public Task<SizeDto> ReadOneSizeById(int id);
        public Task<SizeDto> Update(int id, SizeDto sizeDto);
        public Task<bool> Delete(int id);
      
    }
}
