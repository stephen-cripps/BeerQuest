using System;

namespace BeerQuest.Core
{
    public class Location
    {
         Location()
        { }

        public Location(double lat, double lng)
        {
            if (lat < -90 || lat >90) throw new ArgumentOutOfRangeException(nameof(lat));
            if (lng < -180 || lng > 180) throw new ArgumentOutOfRangeException(nameof(lng));
            Lat = lat;
            Lng = lng;
        }
        public double Lat { get; private set; }
        public double Lng { get; private set; }
    }
}
