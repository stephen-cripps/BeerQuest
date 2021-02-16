using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BeerQuest.Application.Exceptions;
using BeerQuest.Application.Helpers;
using BeerQuest.Application.Storage;
using BeerQuest.Core;
using MediatR;

namespace BeerQuest.Application
{
    /// <summary>
    /// Vertical slice used to query venues
    /// This class contains the input request, the returned view and all code required by this request
    /// Methods should not be shared between vertical slices, in order to decouple requests 
    /// </summary>
    public class GetVenues
    {
        /// <summary>
        /// Defines the query inputs
        /// </summary>
        public class Request : IRequest<IEnumerable<Result>>
        {
            public int MinBeer { get; set; } = 0;
            public int MinAtmosphere { get; set; } = 0;
            public int MinAmenities { get; set; } = 0;
            public int MinValue { get; set; } = 0;
            public double MinLat { get; set; } = -90;
            public double MaxLat { get; set; } = 90;
            public double MinLng { get; set; } = -180;
            public double MaxLng { get; set; } = 180;
            public double Lat { get; set; } = 0;
            public double Lng { get; set; } = 0;
            public int MaxDistKm { get; set; } = 0;
            public string Category { get; set; }
            public string NameSearch { get; set; }
            public List<string> SelectedTags { get; set; } = new List<string>();
        }

        /// <summary>
        /// Defines the returned view
        /// </summary>
        public class Result
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public Uri Url { get; set; }
            public DateTime Date { get; set; }
            public Uri Thumbnail { get; set; }
            public double Lat { get; set; }
            public double Lng { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Twitter { get; set; }
            public double Beer { get; set; }
            public double Atmosphere { get; set; }
            public double Amenities { get; set; }
            public double Value { get; set; }
            public List<string> Tags { get; set; }
        }

        /// <summary>
        /// Handles the request
        /// </summary>
        public class Handler : IRequestHandler<Request, IEnumerable<Result>>
        {
            readonly IVenueRepository repo;
            readonly IMapper mapper;

            public Handler(IVenueRepository repo, IMapper mapper)
            {
                this.repo = repo;
                this.mapper = mapper;
            }

            /// <summary>
            /// This method is run when the request is sent through Mediatr.
            /// It first validates the input, then queries the database, returning the defined view.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public async Task<IEnumerable<Result>> Handle(Request request, CancellationToken token)
            {
                ValidateRequest(request);

                var venues = await repo.QueryVenues(ToExpression(request), token);

                return venues.Select(mapper.Map<Result>);
            }

            /// <summary>
            /// Ensures all input filter parameters are valid
            /// </summary>
            /// <param name="request"></param>
            void ValidateRequest(Request request)
            {
                if (request.MinBeer < 0 || request.MinBeer > 5)
                    throw new InvalidFilterException(nameof(request.MinBeer));
                if (request.MinAtmosphere < 0 || request.MinAtmosphere > 5)
                    throw new InvalidFilterException(nameof(request.MinAtmosphere));
                if (request.MinAmenities < 0 || request.MinAmenities > 5)
                    throw new InvalidFilterException(nameof(request.MinAmenities));
                if (request.MinValue < 0 || request.MinValue > 5)
                    throw new InvalidFilterException(nameof(request.MinValue));

                if (request.MinLat < -90 || request.MinLat > 90 || request.MinLat >= request.MaxLat)
                    throw new InvalidFilterException(nameof(request.MinLat));
                if (request.MaxLat < -90 || request.MaxLat > 90)
                    throw new InvalidFilterException(nameof(request.MaxLat));
                if (request.MinLng < -180 || request.MinLng > 180 || request.MinLng >= request.MaxLng)
                    throw new InvalidFilterException(nameof(request.MinLng));
                if (request.MaxLng < -180 || request.MaxLng > 180)
                    throw new InvalidFilterException(nameof(request.MaxLng));


                if (request.Lat < -90 || request.Lat > 90)
                    throw new InvalidFilterException(nameof(request.Lng));
                if (request.Lng < -180 || request.Lng > 180)
                    throw new InvalidFilterException(nameof(request.Lat));
            }

            /// <summary>
            /// Creates an expression to define whether a particular venue meets matches the input filter requirements.
            /// </summary>
            /// <returns></returns>
            Func<Venue, bool> ToExpression(Request request)
            {
                return venue => venue.Ratings.Beer >= request.MinBeer
                           && venue.Ratings.Atmosphere >= request.MinAtmosphere
                           && venue.Ratings.Amenities >= request.MinAmenities
                           && venue.Ratings.Value >= request.MinValue
                           && venue.Location.Lat >= request.MinLat
                           && venue.Location.Lat <= request.MaxLat
                           && venue.Location.Lng >= request.MinLng
                           && venue.Location.Lng <= request.MaxLng
                           && (Math.Abs(request.MaxDistKm) < 0.1
                               || LocationHelpers.HaversineDistance(
                                   new LatLng(venue.Location.Lat, venue.Location.Lng),
                                   new LatLng(request.Lat, request.Lng)) > request.MaxDistKm)
                           && (request.SelectedTags.Count == 0 || venue.Tags.Any(t => request.SelectedTags.Contains(t)))
                           && (string.IsNullOrEmpty(request.Category) || string.Equals(venue.Category, request.Category, StringComparison.CurrentCultureIgnoreCase))
                           && (string.IsNullOrEmpty(request.NameSearch) || venue.Name.Contains(request.NameSearch, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        /// <summary>
        /// Used to define the AutoMapper mapping from Core.Venue to Result
        /// </summary>
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Venue, GetVenues.Result>()
                    .ForMember(res => res.Lat, opt => opt.MapFrom(v => v.Location.Lat))
                    .ForMember(res => res.Lng, opt => opt.MapFrom(v => v.Location.Lng))
                    .ForMember(res => res.Address, opt => opt.MapFrom(v => v.Contacts.Address))
                    .ForMember(res => res.Phone, opt => opt.MapFrom(v => v.Contacts.Phone))
                    .ForMember(res => res.Twitter, opt => opt.MapFrom(v => v.Contacts.Twitter))
                    .ForMember(res => res.Amenities, opt => opt.MapFrom(v => v.Ratings.Amenities))
                    .ForMember(res => res.Atmosphere, opt => opt.MapFrom(v => v.Ratings.Atmosphere))
                    .ForMember(res => res.Beer, opt => opt.MapFrom(v => v.Ratings.Beer))
                    .ForMember(res => res.Value, opt => opt.MapFrom(v => v.Ratings.Value));
            }
        }
    }
}