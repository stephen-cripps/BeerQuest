using System;
using System.Collections.Generic;
using AutoMapper;
using BeerQuest.Application.Exceptions;
using BeerQuest.Core;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;
using System.Threading;
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
            table.CreateIfNotExists();
        }

        /// <summary>
        /// Gets all venues that return true for a given expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<IEnumerable<Venue>> QueryVenues(Func<Venue, bool> expression, CancellationToken token)
        {
            try
            {
                return Task.FromResult(table.CreateQuery<VenueDto>()
                    .ToList()
                    .Select(dto => mapper.Map<Venue>(dto))
                    .Where(expression));
            }
            catch (Exception e)
            {
                throw new ServiceNotAvailableException(e.Message);
            }
        }


        /// <summary>
        /// Adds or updates multiple venues in batches.
        /// Venues are batched based on their category (up to groups of 100), which is used as the partition key.
        /// </summary>
        /// <param name="venues"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task BatchUpsertVenues(IEnumerable<Venue> venues, CancellationToken token)
        {
            var dtos = venues.Select(v => mapper.Map<VenueDto>(v)).ToList();
            var partitions = dtos.Select(d => d.PartitionKey).Distinct();

            foreach (var partition in partitions)
            {
                var ops = new TableBatchOperation();
                var count = 0;
                foreach (var dto in dtos.Where(d => d.PartitionKey == partition))
                {
                    count++;
                    ops.Add(TableOperation.InsertOrReplace(dto));
                    if (count < 100) continue;

                    try
                    {
                        await table.ExecuteBatchAsync(ops, token);
                    }
                    catch (Exception e)
                    {
                        throw new ServiceNotAvailableException(e.Message);
                    }
                    count = 0;
                    ops = new TableBatchOperation();
                }
                try
                {
                    await table.ExecuteBatchAsync(ops, token);
                }
                catch (Exception e)
                {
                    throw new ServiceNotAvailableException(e.Message);
                }
            }
        }
    }
}
