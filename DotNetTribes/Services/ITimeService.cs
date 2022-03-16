namespace DotNetTribes.Services
{
    public interface ITimeService
    {
        long MinutesSince(long lastUpdateInSeconds);
        long GetCurrentSeconds();
    }
}