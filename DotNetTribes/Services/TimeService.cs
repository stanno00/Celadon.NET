using System;

namespace DotNetTribes.Services
{
    public class TimeService : ITimeService
    {
        public int TimeSince(int timeSince)
        {
            return (int) DateTimeOffset.Now.ToUnixTimeSeconds()/60 - timeSince;
        }
    }
}