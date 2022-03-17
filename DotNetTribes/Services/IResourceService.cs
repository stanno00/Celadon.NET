using System.Collections.Generic;
using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IResourceService
    {
        List<ResourceDto> GetKingdomResources(int kingdomId);
    }
}