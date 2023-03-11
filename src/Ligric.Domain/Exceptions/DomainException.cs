using System;
namespace Ligric.Domain.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message): base(message)
        {
        }

    }
}