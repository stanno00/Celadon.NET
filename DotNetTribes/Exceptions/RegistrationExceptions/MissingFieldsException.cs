using System;

namespace DotNetTribes.RegistrationExceptions
{
    public class MissingFieldsException : Exception
    {
        
        public MissingFieldsException(string message) : base(message)
        {
        }
    }
}