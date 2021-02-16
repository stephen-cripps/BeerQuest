using System;

namespace BeerQuest.Core
{
    public class Ratings
    {
         Ratings()
        { }
        public Ratings(double beer, double atmosphere, double amenities, double value)
        {
            if (beer < 0 || beer > 5) throw new ArgumentOutOfRangeException(nameof(beer));
            if (value < 0 || value > 5) throw new ArgumentOutOfRangeException(nameof(value));
            if (amenities < 0 || amenities > 5) throw new ArgumentOutOfRangeException(nameof(amenities));
            if (atmosphere < 0 || atmosphere > 5) throw new ArgumentOutOfRangeException(nameof(atmosphere));

            Beer = beer;
            Atmosphere = atmosphere;
            Amenities = amenities;
            Value = value;
        }
        public double Beer { get; private set; }
        public double Atmosphere { get; private set; }
        public double Amenities { get; private set; }
        public double Value { get; private set; }
    }
}
