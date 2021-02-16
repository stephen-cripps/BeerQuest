using AutoMapper;
using BeerQuest.Application;
using BeerQuest.Application.Storage;
using BeerQuest.Functions;
using BeerQuest.Tests;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace BeerQuest.
    s
{
    public class TestStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            var services = builder.Services;

            //Mapper
            var assembly = typeof(GetVenues).Assembly;
            services.AddAutoMapper(assembly);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TableStorage.MappingProfile());
                mc.AddProfile(new GetVenues.MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());

            //Mediatr
            services
                .AddMediatR(assembly);

            //Storage (Replaced with mock storage)
            services
                .AddSingleton<IVenueRepository, MockRepository>();
        }
    }
}