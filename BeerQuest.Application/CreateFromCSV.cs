using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BeerQuest.Application.Storage;
using BeerQuest.Core;
using MediatR;

namespace BeerQuest.Application
{
    /// <summary>
    /// Adds fields from a CSV to the Azure Table, ignoring any pre-existing venues
    /// </summary>
    public class CreateFromCsv
    {
        /// <summary>
        /// Defies the request input
        /// </summary>
        public class Request : IRequest<Unit>
        {
            public string Csv { get; set; }
        }

        /// <summary>
        /// Handles the request
        /// </summary>
        public class Handler : IRequestHandler<Request, Unit>
        {
            readonly IVenueRepository repo;

            public Handler(IVenueRepository repo)
            {
                this.repo = repo;
            }

            /// <summary>
            /// This method runs when the request is sent through Mediatr.
            /// It first parses the CSV, then creates the venues, then uploads any venues which don't exist.
            /// In a full implementation that would allow admins to upload CSVs, this would contain data validation on the CSV and
            /// return information on what has and has not been uploaded.
            /// However for the sake of this exercise this has been left out in order to focus on other areas.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public async Task<Unit> Handle(Request request, CancellationToken token)
            {
                var parsedCsv = ParseCsv(request.Csv);
                var venues = CreateVenues(parsedCsv);
                
                await repo.BatchUpsertVenues(venues, token); 

                return Unit.Value;
            }

            /// <summary>
            /// Splits out the CSV rows, trims the "'s from the start and end of each line then splits each row based on a "," delimiter
            /// </summary>
            /// <param name="csv"></param>
            /// <returns></returns>
            IEnumerable<string[]> ParseCsv(string csv) => Regex.Split(csv, "\r\n|\r|\n")
                .Where(row => row.Length > 4)
                .Select(row => row.Substring(1, row.Length - 2))
                .Select(row => Regex.Split(row, "\",\""))
                .ToList();

            /// <summary>
            /// Generates a collection of venues from the input CSV data
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            IEnumerable<Venue> CreateVenues(IEnumerable<string[]> data)
            {
                var headers = data.First();
                var rows = data.Skip(1);

                var venues = new List<Venue>();

                foreach (var row in rows)
                {
                    var dic = new Dictionary<string, string>();
                    for (var i = 0; i < row.Count(); i++)
                    {
                        //Replace any double quotes with single quotes
                        dic[headers[i]] = row[i].Replace("\"\"", "\"");
                    }

                    var ratings = new Ratings(Convert.ToDouble(dic["stars_beer"]),
                        Convert.ToDouble(dic["stars_atmosphere"]), Convert.ToDouble(dic["stars_amenities"]),
                        Convert.ToDouble(dic["stars_value"]));

                    var contacts = new Contacts(dic["address"], dic["phone"], dic["twitter"]);

                    var location = new Location(Convert.ToDouble(dic["lat"]), Convert.ToDouble(dic["lng"]));

                    var tags = dic["tags"].Split(",");

                    venues.Add(new Venue(dic["name"], dic["category"], new Uri(dic["url"]), Convert.ToDateTime(dic["date"]), new Uri(dic["thumbnail"]), location, contacts, ratings, tags));
                }

                return venues;
            }

        }
    }
}
