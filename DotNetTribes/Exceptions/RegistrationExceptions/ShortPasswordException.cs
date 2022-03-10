using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class ShortPasswordException : Exception
    {
        public ShortPasswordException(int minLength) : this($"Password must be at least {minLength} characters")
        {
        }

        public ShortPasswordException(string message) : base(message)
        {
        }
    }
}