using System;

namespace DotNetTribes.Exceptions
{
    public class BuildingCreationException : Exception
    {
        public BuildingCreationException(string errorMessage) : base(errorMessage)
        {
        }
    }
}