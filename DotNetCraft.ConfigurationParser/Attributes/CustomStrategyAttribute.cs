using System;

namespace DotNetCraft.ConfigurationParser.Attributes
{
    /// <summary>
    /// Use this attribute to define a custom mapping strategy to the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomStrategyAttribute: Attribute
    {
        /// <summary>
        /// Strategy's type.
        /// </summary>
        public Type StrategyType { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="strategyType">The strategy's type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="strategyType"/> is <see langword="null"/></exception>
        public CustomStrategyAttribute(Type strategyType)
        {
            if (strategyType == null)
                throw new ArgumentNullException(nameof(strategyType));

            StrategyType = strategyType;
        }
    }
}
