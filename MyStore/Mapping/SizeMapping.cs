﻿using MyStore.DTO;
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
                Description = size.Description,
                CategoryId=size.CategoryId
            };
        }

        // Map a list of Sizes to a list of SizeDtos
        public static List<SizeDto> ToSizeDtoList(this IEnumerable<Size> sizes)
        {
            return sizes.Select(size => size.ToSizeDto()).ToList();
        }
        public static Size ToSize(this SizeDto sizeDto)
        {
            return new Size
            {
                Name = sizeDto.Name,
                Description = sizeDto.Description,
                CategoryId = sizeDto.CategoryId ?? 0
            };
        }
    }
}
