using DotNetTribes.DTOs.Troops;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Services
{
    public interface IBattleService
    {
        Battle Attack(int myKingdomId, int enemyKingdomId, TroopUpgradeRequestDTO troopUpgradeRequestDto);
    }
}