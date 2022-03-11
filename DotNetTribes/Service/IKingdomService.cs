using DotNetTribes.DTOs;

namespace DotNetTribes.Service
{
    public interface IKingdomService
    {
        KingdomDto KingdomInfo(UserDto userDto);
    }
}