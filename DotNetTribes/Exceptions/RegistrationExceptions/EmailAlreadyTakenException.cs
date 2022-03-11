using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class EmailAlreadyTakenException : Exception
    {
        public EmailAlreadyTakenException() : this("Email address is already taken.")
        {
        }

        public EmailAlreadyTakenException(string message) : base(message)
        {
        }
    }
}