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
                .ForMember(dto => dto.lat, opt => opt.MapFrom(v => v.Location.Lat))
                .ForMember(dto => dto.lng, opt => opt.MapFrom(v => v.Location.Lng))
                .ForMember(dto => dto.address, opt => opt.MapFrom(v => v.Contacts.Address))
                .ForMember(dto => dto.phone, opt => opt.MapFrom(v => v.Contacts.Phone))
                .ForMember(dto => dto.twitter, opt => opt.MapFrom(v => v.Contacts.Twitter))
                .ForMember(dto => dto.stars_amenities, opt => opt.MapFrom(v => v.Ratings.Amenities))
                .ForMember(dto => dto.stars_atmosphere, opt => opt.MapFrom(v => v.Ratings.Atmosphere))
                .ForMember(dto => dto.stars_beer, opt => opt.MapFrom(v => v.Ratings.Beer))
                .ForMember(dto => dto.stars_value, opt => opt.MapFrom(v => v.Ratings.Value))
                .ForMember(dto => dto.tags, opt => opt.MapFrom(v => string.Join(",", v.Tags)))
                .ReverseMap()
                .ForMember(v => v.Tags, opt => opt.MapFrom(dto => dto.tags.Split(",", StringSplitOptions.None).ToList()));
        }
    }
}
