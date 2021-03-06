﻿using System;
using System.Xml;

namespace DotNetCraft.ConfigurationParser.Mapping.Strategies
{
    /// <summary>
    /// Interface shows that object is a complex mapping strategy.
    /// </summary>
    public interface IMappingStrategy
    {
        /// <summary>
        /// Convert xml node into the object.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="collectionType">Object's type.</param>
        /// <param name="configurationReader">The IConfigurationReader instance.</param>
        /// <returns>The object.</returns>
        object Map(XmlNode node, Type collectionType, IConfigurationReader configurationReader);
    }
}
