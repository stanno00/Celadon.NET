using System;

namespace DotNetTribes.Exceptions
{
    public class UpgradeException : Exception
    {
        public UpgradeException(string errorMessage) : base(errorMessage)
        {
        }
    }
}