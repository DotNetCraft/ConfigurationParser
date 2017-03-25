using System;
using System.Xml;

namespace DotNetCraft.ConfigurationParser
{
    /// <summary>
    /// Interface shows that object is a configuration reader.
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Read object.
        /// </summary>
        /// <param name="type">The object's type.</param>
        /// <returns>The object.</returns>
        object ReadObject(Type type);

        /// <summary>
        /// Read object.
        /// </summary>
        /// <typeparam name="TObject">The object's type.</typeparam>
        /// <returns>The object.</returns>
        TObject ReadObject<TObject>();

        /// <summary>
        /// Read object from the node.
        /// </summary>
        /// <param name="type">The object's type.</param>
        /// <param name="xmlNode">The node.</param>
        /// <returns>The object.</returns>
        object ReadObject(Type type, XmlNode xmlNode);
    }
}
