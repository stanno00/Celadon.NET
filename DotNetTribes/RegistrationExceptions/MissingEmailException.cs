using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class MissingEmailException : Exception
    {
        public MissingEmailException() : this("Email is required.")
        {
        }

        public MissingEmailException(string message) : base(message)
        {
        }
    }
}