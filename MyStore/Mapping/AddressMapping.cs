﻿using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static class AddressMapping
    {
        public static AddressDto ToAddressDto(this Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                StreetLine1 = address.StreetLine1,
                StreetLine2 = address.StreetLine2,
                City = address.City,
                County = address.County,
                Country = address.Country,
                PostalCode = address.PostalCode,
                AdditionalDetails = address.AdditionalDetails
            };
        }

        // Map a list of addresses to a list of AddressDtos
        public static List<AddressDto> ToAddressDtoList(this IEnumerable<Address> addresses)
        {
            return addresses.Select(address => address.ToAddressDto()).ToList();
        }
    }
}