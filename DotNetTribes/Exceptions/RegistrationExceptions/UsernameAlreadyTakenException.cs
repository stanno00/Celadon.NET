using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class UsernameAlreadyTakenException : Exception
    {
        public UsernameAlreadyTakenException() : this("Username is already taken.")
        {
        }

        public UsernameAlreadyTakenException(string message) : base(message)
        {
        }
    }
}