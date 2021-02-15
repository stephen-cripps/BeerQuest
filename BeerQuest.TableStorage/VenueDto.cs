using System;
using Microsoft.Azure.Cosmos.Table;

namespace BeerQuest.TableStorage
{
    /// <summary>
    /// Mention row key here as exercise shortcut
    /// </summary>
    public class VenueDto : TableEntity
    {
        public string url { get; set; }
        public DateTime date { get; set; }
        public string thumbnail { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string twitter { get; set; }
        public string tags { get; set; }
        public double stars_beer { get; set; }
        public double stars_atmosphere { get; set; }
        public double stars_amenities { get; set; }
        public double stars_value { get; set; }
    }

}
