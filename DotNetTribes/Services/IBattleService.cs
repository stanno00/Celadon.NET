using DotNetTribes.DTOs.Troops;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Services
{
    public interface IBattleService
    {
        Battle UpdateBattles();
        Battle InitializeBattle(int attackerKingdomId, int defenderKingdomId, TroopUpgradeRequestDTO troopsRequestedForBattleDto);
    }
}