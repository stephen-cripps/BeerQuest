using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeerQuest.Application;
using BeerQuest.Functions.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace BeerQuest.Functions
{
    /// <summary>
    /// This class contains all Functions relating to venue data
    /// </summary>
    public class VenueController
    {
        readonly IMediator mediator;

        public VenueController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets all venues based on the input filter parameters in the query string
        /// </summary>
        /// <param name="req"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [FunctionName("GetVenues")]
        public async Task<IActionResult> GetVenues(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req, CancellationToken token)
        {
            try
            {
                var getVenues = req.GetQueryAsObject<GetVenues.Request>();

                if (!string.IsNullOrEmpty(req.Query["tags"].ToString()))
                    getVenues.SelectedTags = req.Query["tags"].ToString().Split(",").ToList();

                return new OkObjectResult(await mediator.Send(getVenues, token));
            }
            catch (JsonException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return ex.ToActionResult();
            }
        }

        /// <summary>
        /// Initialise database on startup. This is only included for the sake of getting data into the Azure table for the exercise.
        /// A full implementation could make this endpoint accessible to admins and include more features such as data handling and bar updates.
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName("TimerTriggerCSharp")]
        public async Task Run([TimerTrigger("0 0 1 1 *", RunOnStartup = true)]
            TimerInfo myTimer, CancellationToken token)
        {
            var createFromCsv = new CreateFromCsv.Request()
            {
                Csv = Properties.Resources.leedsbeerquest
            };

            await mediator.Send(createFromCsv, token);
        }
    }
}
