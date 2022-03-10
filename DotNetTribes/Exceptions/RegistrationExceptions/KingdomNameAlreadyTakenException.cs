using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class KingdomNameAlreadyTakenException : Exception
    {
        public KingdomNameAlreadyTakenException() : this("Kingdom name is already taken.")
        {
        }

        public KingdomNameAlreadyTakenException(string message) : base(message)
        {
        }
    }
}