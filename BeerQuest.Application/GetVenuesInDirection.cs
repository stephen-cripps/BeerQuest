using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BeerQuest.Application.Exceptions;
using BeerQuest.Application.Storage;
using BeerQuest.Core;
using MediatR;

namespace BeerQuest.Application
{
    public class GetVenuesInDirection
    {
        public class Request : IRequest<IEnumerable<GetVenues.Result>>, IRequest<IEnumerable<Result>>
        {
            public double Lng { get; set; }
            public double Lat { get; set; }
            public string Direction { get; set; }
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

        public class Handler : IRequestHandler<Request, IEnumerable<Result>>
        {
            readonly IVenueRepository repo;
            readonly IMapper mapper;

            public Handler(IVenueRepository repo, IMapper mapper)
            {
                this.repo = repo;
                this.mapper = mapper;
            }

            public async Task<IEnumerable<Result>> Handle(Request request, CancellationToken token)
            {
                ValidateRequest(request);

                var venues = await repo.QueryVenues(ToExpression(request), token);

                return venues.Select(mapper.Map<Result>);
            }

            void ValidateRequest(Request request)
            {
                if (request.Lat < -90 || request.Lat > 90)
                    throw new BadRequestException(nameof(request.Lng));
                if (request.Lng < -180 || request.Lng > 180)
                    throw new InvalidFilterException(nameof(request.Lat));
            }

            Func<Venue, bool> ToExpression(Request request)
            {
                switch (request.Direction.ToLower())
                {
                    case "north":
                        return venue => venue.Location.Lat > request.Lat;
                    case "east":
                        return venue => venue.Location.Lng > request.Lng;
                    case "south":
                        return venue => venue.Location.Lat < request.Lat;
                    case "west":
                        return venue => venue.Location.Lng > request.Lng;

                    default: throw new BadRequestException($"{request.Direction} is not a valid direction");
                }
            }
        }
    }
}
