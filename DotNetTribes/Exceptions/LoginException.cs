using System;

namespace DotNetTribes.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException(string yourParameter) : base($"{yourParameter}")
        {
        }
    }
}