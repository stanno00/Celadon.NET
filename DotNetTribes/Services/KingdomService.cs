using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Services
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IResourceService _resourceService;
        private readonly ITimeService _timeService;
        private readonly IRulesService _rules;

        public KingdomService(ApplicationContext applicationContext, IResourceService resourceService,
            ITimeService timeService, IRulesService rules)
        {
            _applicationContext = applicationContext;
            _resourceService = resourceService;
            _timeService = timeService;
            _rules = rules;
        }

        public KingdomDto KingdomInfo(int kingdomId)
        {
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Buildings)
                .Include(k => k.Resources)
                .Include(k => k.Troops)
                .Include(k => k.User)
                .Single(k => k.KingdomId == kingdomId);


            KingdomDto kingdomDto = new KingdomDto()
            {
                KingdomName = kingdom.Name,
                Username = kingdom.User?.Username,
                Buildings = kingdom.Buildings,
                Resources = _resourceService.GetKingdomResources(kingdomId),
                Troops = kingdom.Troops
            };
            return kingdomDto;
        }
    }
}