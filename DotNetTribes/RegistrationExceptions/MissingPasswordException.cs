using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class MissingPasswordException : Exception
    {
        public MissingPasswordException() : this("Password is required.")
        {
        }

        public MissingPasswordException(string message) : base(message)
        {
        }
    }
}