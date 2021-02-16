using System;
using System.Collections.Generic;

namespace BeerQuest.Core
{
    public class Venue
    {

         Venue()
        { }

        public Venue(string name, string category, Uri url, DateTime date, Uri thumbnail, Location location, Contacts contacts, Ratings ratings, IEnumerable<string> tags)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Url = url;
            Date = date;
            Thumbnail = thumbnail;
            Location = location ?? throw new ArgumentNullException(nameof(location));
            Contacts = contacts;
            Ratings = ratings;
            Tags = tags;
        }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public Uri Url { get; private set; }
        public DateTime Date { get; private set; }
        public Uri Thumbnail { get; private set; }
        public Location Location { get; private set; }
        public Contacts Contacts { get; private set; }
        public Ratings Ratings { get; private set; }
        public IEnumerable<string> Tags { get; private set; }
    }
}
