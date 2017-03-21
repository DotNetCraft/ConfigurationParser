using System;

namespace ConfigurationParser.Mapping
{
    /// <summary>
    /// Interface shows that object is a mapping strategies factory.
    /// </summary>
    public interface IMappingStrategyFactory
    {
        /// <summary>
        /// Create a primitive mapping strategy by object's type.
        /// </summary>
        /// <param name="itemType">The object's type.</param>
        /// <returns>The IPrimitiveMappingStrategy instance.</returns>
        IPrimitiveMappingStrategy CreatePrimitiveStrategy(Type itemType);

        /// <summary>
        /// Create a complex mapping strategy by object's type.
        /// </summary>
        /// <param name="itemType">The object's type.</param>
        /// <returns>The IMappingStrategy instance.</returns>
        IMappingStrategy CreateComplexStrategy(Type itemType);

        /// <summary>
        /// Register mapping strategy using object's type.
        /// </summary>
        /// <param name="itemType">The object's type.</param>
        /// <param name="mappingStrategy">The <see cref="IMappingStrategy"/> instance.</param>
        void Register(Type itemType, IMappingStrategy mappingStrategy);
    }
}
