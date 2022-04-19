using System;

namespace DotNetTribes.Exceptions
{
    public class KingdomNotFoundException : Exception
    {
        public KingdomNotFoundException(string message) : base(message)
        {
            
        }
    }
}