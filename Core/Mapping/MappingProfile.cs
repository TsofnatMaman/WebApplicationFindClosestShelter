using System.Data;
using System.Xml.Serialization;
using AutoMapper;
using Core.Entities;
using Core.Models;
using Core.Resources_DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Location mappings
        CreateMap<Location, string>()
           .ConvertUsing(src => $"{src.Longitude},{src.Latitude}");

        CreateMap<string, Location>().ConvertUsing((src, context) =>
        {
            var parts = src.Split(',');
            return new Location
            {
                Longitude = double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture),
                Latitude = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture)
            };
        });


        // Address mappings
        CreateMap<Address, AddressDTO>()
            .ForMember(dest => dest.Location,
                       opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Shelter,
                       opt => opt.MapFrom(src => src.Shelter));

        CreateMap<AddressDTO, Address>()
            .ForMember(dest => dest.Location,
                    opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Shelter, opt => opt.Ignore())
            .ForMember(dest => dest.ShelterCode, opt => opt.MapFrom(src => src.Shelter.Code));


        // Opinion mappings
        CreateMap<Opinion, OpinionDTO>();
        CreateMap<OpinionDTO, Opinion>()
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.AddressCode, opt=>opt.MapFrom(src => src.Address.Code));

        // Shelter mappings

        //CreateMap<ShelterDTO, Shelter>()
        //    .ForMember(dest => dest.Name,
        //               opt => opt.MapFrom(src => Enum.Parse<ShelterTypes>(src.NameStr)));

        //CreateMap<Shelter, ShelterDTO>()
        //    .ForMember(dest => dest.NameStr,
        //               opt => opt.MapFrom(src => src.Name.ToString()));

        CreateMap<Shelter, ShelterDTO>()
            .ForMember(dest => dest.NameStr, opt => opt.MapFrom(src => src.Name.ToString()))
            .ReverseMap();
    }
}
