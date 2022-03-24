using System;

namespace DotNetTribes.Exceptions
{
    public class TradeException : Exception
    {
        public TradeException(string yourParameter) : base($"{yourParameter}")
        {
        }
    }
}