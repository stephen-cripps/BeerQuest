using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BeerQuest.Application;
using BeerQuest.Application.Storage;
using BeerQuest.Core;
using Moq;
using Xunit;

namespace BeerQuest.Tests
{
    /// <summary>
    /// As mentioned in the readme, I did not have time to build out all of the Unit and Functional Tests. However I've included some examples. 
    /// </summary>
    public class UnitTestExample
    {
        readonly Mock<IVenueRepository> mockRepo = new Mock<IVenueRepository>();
        readonly IMapper mapper;

        public UnitTestExample()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TableStorage.MappingProfile());
                mc.AddProfile(new GetVenues.MappingProfile());
            });
             mapper = mappingConfig.CreateMapper();
        }
        /// <summary>
        /// Tests that when latitude is outside of the valid range, and argument out of range exception is thrown
        /// </summary>
        [Fact]
        public void LocationCtor_InvalidLat_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>new Location(91, 50));
        }

        /// <summary>
        /// Ensures that get venues can safely return an empty list
        /// </summary>
        [Fact]
        public async Task GetVenuesHandle_NoVenuesExist_ReturnsEmptyList()
        {
            //Arrange
            var expression = new Func<Venue, bool>(v => false);
            IEnumerable<Venue> response = new List<Venue>();
            mockRepo.Setup(m => m.QueryVenues(expression, new CancellationToken())).Returns(Task.FromResult(response)); 

            var handler = new GetVenues.Handler(mockRepo.Object, mapper);
            var request = new GetVenues.Request();

            //Test
            var result = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Empty(result);
        }
    }
}
