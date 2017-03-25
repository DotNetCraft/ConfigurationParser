using System;
using System.Collections.Generic;
using System.Xml;
using DotNetCraft.ConfigurationParser.Mapping.Strategies;

namespace DotNetCraft.ConfigurationParser.Example.CustomStrategies
{
    class SplitRecipientsCustomStrategy : ICustomMappingStrategy
    {
        #region Implementation of ICustomMappingStrategy

        /// <summary>
        /// Convert input into the object.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="itemType">Object's type.</param>
        /// <returns>The object.</returns>
        public object Map(string input, Type itemType)
        {
            string[] items = input.Split(';');
            List<string> result = new List<string>();

            result.AddRange(items);

            return result;
        }

        /// <summary>
        /// Convert xml node into the object.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="itemType">Object's type.</param>
        /// <returns>The object.</returns>
        public object Map(XmlNode xmlNode, Type itemType)
        {
            string input = xmlNode.InnerText;
            return Map(input, itemType);
        }

        #endregion
    }
}
