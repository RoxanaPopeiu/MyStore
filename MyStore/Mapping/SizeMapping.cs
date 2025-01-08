using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static class SizeMapping
    {
        // Map Size to SizeDto
        public static SizeDto ToSizeDto(this Size size)
        {
            return new SizeDto
            {
                Id = size.Id,
                Name = size.Name,
                Description = size.Description
            };
        }

        // Map a list of Sizes to a list of SizeDtos
        public static List<SizeDto> ToSizeDtoList(this IEnumerable<Size> sizes)
        {
            return sizes.Select(size => size.ToSizeDto()).ToList();
        }
    }
}
