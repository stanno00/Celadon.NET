using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IResourcesService
    {
        ResourcesDto GetKingdomResources(int kingdomId);
    }
}