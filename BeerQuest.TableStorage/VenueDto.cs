using System;
using Microsoft.Azure.Cosmos.Table;

namespace BeerQuest.TableStorage
{
    /// <summary>
    /// A flat version of the Venue object to store in a table
    /// Name and Category are used as RowKey and PartitionKey respectively
    /// </summary>
    public class VenueDto : TableEntity
    {
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public string Thumbnail { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Twitter { get; set; }
        public string Tags { get; set; }
        public double Beer { get; set; }
        public double Atmosphere { get; set; }
        public double Amenities { get; set; }
        public double Value { get; set; }
    }

}
