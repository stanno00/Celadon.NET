﻿using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IKingdomService
    {
        KingdomDto KingdomInfo(int kingdomId);
    }
}