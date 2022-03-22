using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DotNetTribes.DTOs;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IResourceService _resourceService;

        public KingdomService(ApplicationContext applicationContext, IResourceService resourceService)
        {
            _applicationContext = applicationContext;
            _resourceService = resourceService;
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

        public object NearestKingdoms( int minutes,int kingdomId)
        {
            // todo if minutes are less than 2 return error message It takes at least two minutes to move one square!

            var kingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == kingdomId);

            List<NearbyKingdomsDto> kingdomsDtos = new List<NearbyKingdomsDto>();

            var mapStartingXCoordinates = kingdom.KingdomX - (minutes / 2);
            var mapStartingYCoordinates = kingdom.KingdomY - (minutes / 2);
            
            var minutesDecreaseX = DecreasingOptionsX(mapStartingXCoordinates);
            var minutesDecreaseY = DecreasingOptionsY(mapStartingYCoordinates);
            
            mapStartingXCoordinates = DecreasingPossibleOptionsX(mapStartingXCoordinates);
            mapStartingYCoordinates = DecreasingPossibleOptionsY(mapStartingYCoordinates);
            
            if (kingdom.KingdomX == 99) { minutesDecreaseX--; }

            if (kingdom.KingdomY == 99) { minutesDecreaseY--; }

            for (int i = mapStartingYCoordinates; i <= mapStartingYCoordinates + (minutes - minutesDecreaseY); i++)
            {
                for (int j = mapStartingXCoordinates; j <= mapStartingXCoordinates + (minutes - minutesDecreaseX); j++)
                {
                    if (_applicationContext.Kingdoms.Any(k => k.KingdomX == j && k.KingdomY == i))
                    {
                        var kingdomFound = _applicationContext.Kingdoms
                            .Single(k => k.KingdomX == j && k.KingdomY == i && kingdomId != k.KingdomId);
                        
                        kingdomsDtos.Add(new NearbyKingdomsDto()
                        {
                            KingdomId = kingdomFound.KingdomId,
                            KingdomName = kingdomFound.Name,
                            KingdomCoordinateX = kingdomFound.KingdomX,
                            KingdomCoordinateY = kingdomFound.KingdomY
                        });
                    }
                }
            }

            return kingdomsDtos;
        }

        public int ShortestPath(int myKingdomId, int attackOn)
        {
            int steps = 0;
            
            var myKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == myKingdomId);
            var kingdomUnderAttack = _applicationContext.Kingdoms.Single(k => k.KingdomId == attackOn);

            var myKingdomX = myKingdom.KingdomX;
            var myKingdomY = myKingdom.KingdomY;
            
            var kingdomUnderAttackX = kingdomUnderAttack.KingdomX;
            var kingdomUnderAttackY = kingdomUnderAttack.KingdomY;

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is top left
                if (myKingdomX > kingdomUnderAttackX && myKingdomY > kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX || myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX--;
                        myKingdomY--;
                        steps++;
                    }
                }
            }

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is top right
                if (myKingdomX < kingdomUnderAttackX && myKingdomY > kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX || myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX++;
                        myKingdomY--;
                        steps++;
                    }
                }
            }

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is bottom left
                if (myKingdomX > kingdomUnderAttackX && myKingdomY < kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX || myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX--;
                        myKingdomY++;
                        steps++;
                    }
                }
            }

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is bottom right
                if (myKingdomX < kingdomUnderAttackX && myKingdomY < kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX || myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX++;
                        myKingdomY++;
                        steps++;
                    }
                }
            }

            // kingdoms are on the same X 
            if (myKingdomX == kingdomUnderAttackX && myKingdomY > kingdomUnderAttackY)
            {
                while (myKingdomY != kingdomUnderAttackY)
                {
                    myKingdomY--;
                    steps++;
                }
            }
            
            if (myKingdomX == kingdomUnderAttackX && myKingdomY < kingdomUnderAttackY)
            {
                while (myKingdomY != kingdomUnderAttackY)
                {
                    myKingdomY++;
                    steps++;
                }
            }
            
            // kingdoms are on the same Y
            if (myKingdomY == kingdomUnderAttackY && myKingdomX > kingdomUnderAttackX)
            {
                while (myKingdomX != kingdomUnderAttackX)
                {
                    myKingdomX--;
                    steps++;
                }
            }
            if (myKingdomY == kingdomUnderAttackY && myKingdomX < kingdomUnderAttackX)
            {
                while (myKingdomX != kingdomUnderAttackX)
                {
                    myKingdomX++;
                    steps++;
                }
            }

            return steps;
        }
        
        private static int DecreasingOptionsY(int mapStartingYCoordinates)
        {
            var result = 0;
            if (mapStartingYCoordinates < 0) { return Math.Abs(mapStartingYCoordinates); }

            if (mapStartingYCoordinates >= 100) { result = mapStartingYCoordinates - 100; }

            return Math.Abs(result);
        }

        private static int DecreasingOptionsX(int mapStartingXCoordinates)
        {
            var result = 0;
            if (mapStartingXCoordinates < 0) { return Math.Abs(mapStartingXCoordinates); }

            if (mapStartingXCoordinates >= 100) { result = mapStartingXCoordinates - 100; }

            return Math.Abs(result);
        }
        
        private static int DecreasingPossibleOptionsX(int x)
        {
            if (x < 0) { x = 0; }
            if (x > 100) { x = 100; }
            return x;
        }
        
        private static int DecreasingPossibleOptionsY(int y)
        {
            if (y < 0) { y = 0; }
            if (y > 100) { y = 100; }
            return y;
        }
    }
}