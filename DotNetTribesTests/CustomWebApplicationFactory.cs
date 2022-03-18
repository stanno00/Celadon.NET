using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetTribesTests;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationContext>));

            services.Remove(descriptor);
            string dbname = Guid.NewGuid().ToString();

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase(dbname);
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();

                db.Users.Add(new User()
                {
                    Email = "realEmail@Test.dummy",
                    Username = "Hrnik",
                    UserId = 1,
                    HashedPassword = "$2a$11$EtdJ7HIRZihSF/WLmYf8HOnGD1VThPLGV3lg4PGnVan4IvOXD0.Ru",
                    Kingdom = new Kingdom()
                    {
                        Name = "Cool Kingdom Name",
                        KingdomId = 1,
                        Buildings = new List<Building>(),
                        Resources = new List<Resource>(),
                        Troops = new List<Troop>(),
                    },
                    KingdomId = 1
                });
                db.SaveChanges();

                db.Database.EnsureCreated();
            }
        });
    }
}