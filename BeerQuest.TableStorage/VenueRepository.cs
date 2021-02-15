using System;
using System.Collections.Generic;
using AutoMapper;
using BeerQuest.Application.Exceptions;
using BeerQuest.Core;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;
using System.Threading.Tasks;
using BeerQuest.Application.Storage;

namespace BeerQuest.TableStorage
{
    /// <summary>
    /// Repository used to access Venue data in Azure Table Storage
    /// </summary>
    public class VenueRepository : IVenueRepository
    {
        readonly CloudTable table;
        readonly IMapper mapper;

        public VenueRepository(CloudTableClient client, IMapper mapper)
        {
            this.mapper = mapper;

            table = client.GetTableReference(Environment.GetEnvironmentVariable("venues-table"));
            if (!table.Exists())
                throw new ServiceNotAvailableException("venues-table does not exist");
        }

        /// <summary>
        /// Gets all venues that return true for a given expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<IEnumerable<Venue>> QueryVenues(Func<Venue, bool> expression)
        {
            var test = table.CreateQuery<VenueDto>()
                .ToList();

            return Task.FromResult(table.CreateQuery<VenueDto>()
                .ToList()
                .Select(dto => mapper.Map<Venue>(dto))
                .Where(expression));
        }
    }
}
