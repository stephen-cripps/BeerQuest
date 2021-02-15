using System;
using System.Collections.Generic;

namespace BeerQuest.Core
{
    public class Venue
    {
        public string Name { get; private set; }
        public string Category { get; private set; }
        public Uri Url { get; private set; }
        public DateTime Date { get; private set; }
        public Uri Thumbnail { get; private set; }
        public Location Location { get; private set; }
        public Contacts Contacts { get; private set; }
        public Ratings Ratings { get; private set; }
        public List<string> Tags { get; private set; }
    }
}
