﻿using System;

namespace ConfigurationParser.Attributes
{
    /// <summary>
    /// Use this attribute to define a property that will be used for storing data form the xml element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyMappingAttribute : Attribute
    {
        /// <summary>
        /// Xml element name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Xml element name.</param>
        public PropertyMappingAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }
    }
}
