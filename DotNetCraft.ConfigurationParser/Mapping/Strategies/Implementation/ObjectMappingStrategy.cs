using System;
using System.Xml;

namespace DotNetCraft.ConfigurationParser.Mapping.Strategies.Implementation
{
    /// <summary>
    /// The strategy is using for converting node data into the object
    /// </summary>
    public class ObjectMappingStrategy : IMappingStrategy
    {
        #region Implementation of IMappingStrategy

        /// <summary>
        /// Convert xml node into the object.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="itemType">Object's type.</param>
        /// <param name="configurationReader">The IConfigurationReader instance.</param>
        /// <returns>The object.</returns>
        public object Map(XmlNode node, Type itemType, IConfigurationReader configurationReader)
        {
            var value = configurationReader.ReadObject(itemType, node);
            return value;
        }

        #endregion
    }
}
