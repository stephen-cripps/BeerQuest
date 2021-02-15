using System;
using AutoMapper;
using BeerQuest.Application;
using BeerQuest.Application.Storage;
using BeerQuest.Functions;
using BeerQuest.TableStorage;
using MediatR;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace BeerQuest.Functions
{
    public class Startup : FunctionsStartup
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

            //Storage
            services
                .AddTransient<IVenueRepository, VenueRepository>()
                .AddTransient(sp =>
                {
                    var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

                    var storageAccount = CloudStorageAccount.Parse(connectionString);

                    return storageAccount.CreateCloudTableClient();
                });
        }
    }
}