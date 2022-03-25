using System;

namespace DotNetTribes.Exceptions
{
    public class TroopCreationException : Exception
    {
        public TroopCreationException(string message) : base(message)
        {
        }
    }
}