using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IResourceService
    {
        ResourcesDto GetKingdomResources(int kingdomId);
    }
}