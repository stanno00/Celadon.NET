using System;

namespace DotNetTribes.Exceptions
{
    public class SampleException : Exception
    {
        public SampleException() : base ("This is a sample exception.")
        {
        }

        public SampleException(string yourParameter) : base($"This is a sample message using {yourParameter}.")
        {
        }
    }
}