using System;

namespace BeerQuest.Application.Helpers
{
    /// <summary>
    /// Helper functions for handling Lat/Long coordinates
    /// </summary>
    public static class LocationHelpers
    {
        /// <summary>
        /// Returns the distance in kilometers of any two
        /// latitude / longitude points.
        /// Borrowed from stormconsultancy.co.uk
        /// </summary>
        /// <param name="pos1">Location 1</param>
        /// <param name="pos2">Location 2</param>
        /// <returns>Distance in the requested unit</returns>
        public static  double HaversineDistance(LatLng pos1, LatLng pos2)
        {
            const double r = 6371;
            var lat = (pos2.Latitude - pos1.Latitude).ToRadians();
            var lng = (pos2.Longitude - pos1.Longitude).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(pos1.Latitude.ToRadians()) * Math.Cos(pos2.Latitude.ToRadians()) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return r * h2;
        }
    }

    /// <summary>
    /// Specifies a Latitude / Longitude point.
    /// </summary>
    public class LatLng
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LatLng()
        {
        }

        public LatLng(double lat, double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }
    }

    /// <summary>
    /// Convert to Radians.
    /// </summary>
    /// <param name="val">The value to convert to radians</param>
    /// <returns>The value in radians</returns>
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}