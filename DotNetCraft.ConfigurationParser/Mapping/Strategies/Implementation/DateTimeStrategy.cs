﻿using System;
using System.Xml;

namespace DotNetCraft.ConfigurationParser.Mapping.Strategies.Implementation
{
    public class DateTimeStrategy : IMappingStrategy
    {
        #region Implementation of IMappingStrategy

        /// <summary>
        /// Convert xml node into the object.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="collectionType">Object's type.</param>
        /// <param name="configurationReader">The IConfigurationReader instance.</param>
        /// <returns>The object.</returns>
        public object Map(XmlNode node, Type collectionType, IConfigurationReader configurationReader)
        {
            string input = node.InnerText;
            DateTime result = DateTime.Parse(input);
            return result;
        }

        #endregion
    }
}
