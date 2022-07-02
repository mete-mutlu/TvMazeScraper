using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Infrastructure.EntityFramework;
using TvMazeScraper.Core.Entities;
using TvMazeScraper.Api.BackgroundServices;

namespace Api.IntegrationTests
{

    public class CustomWebApplicationFactory<TStartup>
     : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                var descriptor = services.SingleOrDefault(
                 d => d.ServiceType ==
                     typeof(DbContextOptions<ScraperDbContext>));

                services.Remove(descriptor);


                var scheduler = services.Single(s => s.ImplementationType == typeof(SchedulerService));
                
                services.Remove(scheduler);


                services.Remove(descriptor);


                services.AddDbContext<ScraperDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ScraperDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        db.Shows.Add(new Show { Id = 1, Name = "Show1" });

                        db.Shows.Add(new Show { Id = 2, Name = "Show2" });


                        db.Persons.Add(new Person {Id= 1, Name = "Name1", BirthDay = DateTime.Now.AddYears(-25)});
                        db.Persons.Add(new Person { Id = 2, Name = "Name2", BirthDay = DateTime.Now.AddYears(-42) });
                        db.Persons.Add(new Person { Id = 3, Name = "Name3", BirthDay = DateTime.Now.AddYears(-58) });
                        db.Persons.Add(new Person { Id = 4, Name = "Name4", BirthDay = DateTime.Now.AddYears(-76) });
                        db.Persons.Add(new Person { Id = 5, Name = "Name5", BirthDay = DateTime.Now.AddYears(-34) });



                        db.Casts.Add(new Cast { Id = 1, PersonId = 1, ShowId = 1 });

                        db.Casts.Add(new Cast { Id = 2, PersonId = 2, ShowId = 1 });

                        db.Casts.Add(new Cast { Id = 3, PersonId = 3, ShowId = 1 });


                        db.Casts.Add(new Cast { Id = 4, PersonId = 3, ShowId = 2 });

                        db.Casts.Add(new Cast { Id = 5, PersonId = 4, ShowId = 2 });

                        db.Casts.Add(new Cast { Id = 6, PersonId = 5, ShowId = 2 });
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}