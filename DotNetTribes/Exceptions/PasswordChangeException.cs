using System;

namespace DotNetTribes.Exceptions
{
    public class PasswordChangeException : Exception
    {
        public PasswordChangeException(string yourParameter) : base($"{yourParameter}")
        {
        }
    }
}