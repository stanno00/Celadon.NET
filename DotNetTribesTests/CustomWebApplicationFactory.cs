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

                db.GameRules.Add(new GameRules
                {
                    GameRulesId = 1,
                    Name = "Production",
                    StartingGold = 500,
                    StartingFood = 500,
                    TownhallAllLevelsCost = 200,
                    FarmAllLevelsCost = 100,
                    MineAllLevelsCost = 100,
                    AcademyLevelOneCost = 150,
                    AcademyLevelNCost = 100,
                    TroopAllLevelsCost = 25,
                    TownhallLevelOneDuration = 120,
                    TownhallLevelNDuration = 60,
                    FarmAllLevelsDuration = 60,
                    FarmAllLevelsFoodGeneration = 5,
                    MineAllLevesDuration = 60,
                    MineALlLevelsGoldGeneration = 5,
                    AcademyLevelOneDuration = 90,
                    AcademyLevelNDuration = 60,
                    TroopAllLevelsDuration = 30,
                    TownhallHP = 200,
                    FarmHP = 100,
                    MineHP = 100,
                    AcademyHP = 150,
                    TroopHP = 20,
                    MapBoundariesX = 101,
                    MapBoundariesY = 101
                });
                
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