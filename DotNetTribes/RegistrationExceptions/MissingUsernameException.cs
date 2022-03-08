using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class MissingUsernameException : Exception
    {
        public MissingUsernameException() : this("Username is required.")
        {
        }

        public MissingUsernameException(string message) : base(message)
        {
        }
    }
}