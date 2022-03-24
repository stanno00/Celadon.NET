using System;

namespace DotNetTribes.Exceptions
{
    public class KingdomReceiveMinutesLessThanTwo : Exception
    {
        public KingdomReceiveMinutesLessThanTwo(string errorMessage) : base(errorMessage)
        {
        }
    }
}