using System;

namespace DotNetCraft.ConfigurationParser.Exceptions
{
    /// <summary>
    /// This type of exception can be raised only from the MappingStrategyFactory instance.
    /// </summary>
    public class MappingStrategyFactoryException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The message</param>
        public MappingStrategyFactoryException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">Inner exception.</param>
        public MappingStrategyFactoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
