using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeerQuest.Application;
using BeerQuest.Functions;
using BeerQuest.s;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace BeerQuest.Tests
{

    /// <summary>
    /// As mentioned in the readme, I did not have time to build out all of the Unit and Functional Tests. However I've included some examples. 
    /// </summary>
    public class FunctionalTestExample
    {
        readonly VenueController controller;
        public FunctionalTestExample()
        {
            var startup = new TestStartup();
            var host = new HostBuilder()
                .ConfigureWebJobs(startup.Configure)
                .Build();

            controller = new VenueController(host.Services.GetRequiredService<IMediator>());
        }

        /// <summary>
        /// Tests the GetVenues request, searching for any name with 3 in it
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetVenues_WithNameSearch_ReturnsSingleResult()
        {
            //Arrange
            //Create a Httprequest with a querystring of namesearch=3
            var req = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>() {{"namesearch", "3"}})      
            };
            
            //Test
            var resp = (await controller.GetVenues(req, new CancellationToken())) as ObjectResult;

            //Assert
            Assert.Equal(200, resp.StatusCode);

            var result = resp.Value as IEnumerable<GetVenues.Result>;
            Assert.Equal("venue: 3",result.Single().Name);
        }
    }
}
