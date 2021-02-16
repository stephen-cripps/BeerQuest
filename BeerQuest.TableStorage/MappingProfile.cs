using System;
using System.Linq;
using AutoMapper;
using BeerQuest.Core;

namespace BeerQuest.TableStorage
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Defines the mapping between Core.Venue and VenueDto
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Venue, VenueDto>()
                .ForMember(dto => dto.RowKey, opt => opt.MapFrom(v => v.Name))
                .ForMember(dto => dto.PartitionKey, opt => opt.MapFrom(v => v.Category))
                .ForMember(dto => dto.Lat, opt => opt.MapFrom(v => v.Location.Lat))
                .ForMember(dto => dto.Lng, opt => opt.MapFrom(v => v.Location.Lng))
                .ForMember(dto => dto.Address, opt => opt.MapFrom(v => v.Contacts.Address))
                .ForMember(dto => dto.Phone, opt => opt.MapFrom(v => v.Contacts.Phone))
                .ForMember(dto => dto.Twitter, opt => opt.MapFrom(v => v.Contacts.Twitter))
                .ForMember(dto => dto.Amenities, opt => opt.MapFrom(v => v.Ratings.Amenities))
                .ForMember(dto => dto.Atmosphere, opt => opt.MapFrom(v => v.Ratings.Atmosphere))
                .ForMember(dto => dto.Beer, opt => opt.MapFrom(v => v.Ratings.Beer))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(v => v.Ratings.Value))
                .ForMember(dto => dto.Tags, opt => opt.MapFrom(v => string.Join(",", v.Tags)))
                .ReverseMap()
                .ForMember(v => v.Tags, opt => opt.MapFrom(dto => dto.Tags.Split(",", StringSplitOptions.None).ToList()));
        }
    }
}
