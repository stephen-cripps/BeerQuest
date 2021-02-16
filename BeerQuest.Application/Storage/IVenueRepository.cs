using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BeerQuest.Core;

namespace BeerQuest.Application.Storage
{
    /// <summary>
    /// Repository used to access venue data
    /// </summary>
    public interface IVenueRepository
    {
        /// <summary>
        /// Gets all venues that return true for a given expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<Venue>> QueryVenues(Func<Venue, bool> expression, CancellationToken token);

        /// <summary>
        /// Adds or updates multiple venues.
        /// </summary>
        /// <param name="venues"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task BatchUpsertVenues(IEnumerable<Venue> venues, CancellationToken token);
    }
}
