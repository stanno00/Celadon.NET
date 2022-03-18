using System;

namespace DotNetTribes.Services
{
    public class TimeService : ITimeService
    {
        public long MinutesSince(long lastUpdateInSeconds)
        {
            return (GetCurrentSeconds() - lastUpdateInSeconds) / 60;
        }

        public long SecondsSince(long lastUpdateInSeconds)
        {
            return GetCurrentSeconds() - lastUpdateInSeconds;
        }

        public long GetCurrentSeconds()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}