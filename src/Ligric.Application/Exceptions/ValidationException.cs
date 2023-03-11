using Ligric.Domain.Exceptions;

namespace Ligric.Application.Exceptions
{

    /// <summary>
    /// 
    /// </summary>
    public class ValidationException : DomainException
    {
        #region Ctors

        /// <summary>
        /// 
        /// </summary>
        public ValidationException() : base("")
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public ValidationException(string message) : base(message)
        {
        }


        #endregion
    }
}