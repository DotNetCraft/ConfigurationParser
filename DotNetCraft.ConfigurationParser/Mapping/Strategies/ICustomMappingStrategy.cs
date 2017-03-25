using System;
using System.Xml;

namespace DotNetCraft.ConfigurationParser.Mapping.Strategies
{
    /// <summary>
    /// Interface shows that object is a custom mapping strategy.
    /// </summary>
    public interface ICustomMappingStrategy
    {
        /// <summary>
        /// Convert input into the object.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="itemType">Object's type.</param>
        /// <returns>The object.</returns>
        object Map(string input, Type itemType);

        /// <summary>
        /// Convert xml node into the object.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="itemType">Object's type.</param>
        /// <returns>The object.</returns>
        object Map(XmlNode xmlNode, Type itemType);
    }
}
