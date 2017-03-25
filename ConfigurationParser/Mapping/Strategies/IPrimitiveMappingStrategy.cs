using System;

namespace ConfigurationParser.Mapping.Strategies
{
    /// <summary>
    /// Interface shows that object is a primitive mapping strategy.
    /// </summary>
    public interface IPrimitiveMappingStrategy
    {
        /// <summary>
        /// Convert input string into the object.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="itemType">Object's type.</param>
        /// <returns>The object.</returns>
        object Map(string input, Type itemType);
    }
}
