using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeerQuest.Application.Storage;
using BeerQuest.Core;

namespace BeerQuest.Tests
{
    class MockRepository : IVenueRepository
    {
        public List<Venue> Venues;

        /// <summary>
        /// Initialise some mock venues
        /// </summary>
        public MockRepository()
        {
            Venues = new List<Venue>();
            for (var i = 0; i < 5; i++)
            {
                var contacts = new Contacts("add: " + i, "phone: " + i, "twit: " + i);
                var location = new Location(i, i);
                var ratings = new Ratings(i, i, i, i);
                var venue = new Venue("venue: " + i, "cat: " + i, new Uri("http://url.com"), DateTime.Now,
                    new Uri("http://thumb.com"), location, contacts, ratings, new List<string>());

                Venues.Add(venue);
            }
        }

        public Task<IEnumerable<Venue>> QueryVenues(Func<Venue, bool> expression, CancellationToken token)
        {
            return Task.FromResult(Venues.Where(expression));
        }

        public Task BatchUpsertVenues(IEnumerable<Venue> venues, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
