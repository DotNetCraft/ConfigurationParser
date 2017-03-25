using System;
using System.ComponentModel;

namespace ConfigurationParser.Mapping.Strategies.Implementation
{
    /// <summary>
    /// The strategy is using for converting node data into the primitive object
    /// </summary>
    public class PrimitiveMappingStrategy : IPrimitiveMappingStrategy
    {
        #region Implementation of IPrimitiveMappingStrategy

        /// <summary>
        /// Convert input string into the primitive object.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="itemType">Object's type.</param>
        /// <returns>The primitive object.</returns>
        /// <exception cref="NotSupportedException">Only primitive types are supported.</exception>
        public object Map(string input, Type itemType)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(itemType);

            if (converter.CanConvertFrom(typeof(string)) == false)
            {
                string msg = string.Format("Cannot convert {0} into the {1}", input, itemType);
                throw new NotSupportedException(msg);
            }

            var value = converter.ConvertFromString(input);
            return value;
        }

        #endregion
    }
}
