using System;

namespace DotNetTribes.Exceptions
{
    public class KingdomDoesNotExistException : Exception
    {
        public KingdomDoesNotExistException() : base("Kingdom with this Id does not exist")
        {
            
        }

        public KingdomDoesNotExistException(string yourParameter) : base($"This is a sample message using {yourParameter}.")
        {
            
        }
    }
}