using System.Xml;

namespace ConfigurationParser
{
    /// <summary>
    /// Xml node extention methods.
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// Get attribute value by the attribute's name.
        /// </summary>
        /// <param name="xmlNode">The XmlNode instance.</param>
        /// <param name="attributeName">The attribute's name.</param>
        /// <returns>The value.</returns>
        public static string GetAttributeValue(this XmlNode xmlNode, string attributeName)
        {
            string result = null;

            if (xmlNode.Attributes != null && xmlNode.Attributes[attributeName] != null)
                result = xmlNode.Attributes[attributeName].Value;

            return result;
        }

        /// <summary>
        /// Get xml child node inner text by the name.
        /// </summary>
        /// <param name="xmlNode">The XmlNode instance.</param>
        /// <param name="nodeName">The node's name.</param>
        /// <returns>The inner text..</returns>
        public static string GetElementValue(this XmlNode xmlNode, string nodeName)
        {
            for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                XmlNode childNode = xmlNode.ChildNodes[i];
                if (childNode.Name == nodeName)
                    return childNode.InnerText;
            }

            return null;
        }

        /// <summary>
        /// Get child node by the name
        /// </summary>
        /// <param name="xmlNode">The XmlNode instance.</param>
        /// <param name="nodeName">The node's name.</param>
        /// <returns>The node.</returns>
        public static XmlNode GetXmlNode(this XmlNode xmlNode, string nodeName)
        {
            for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                XmlNode childNode = xmlNode.ChildNodes[i];
                if (childNode.Name == nodeName)
                    return childNode;
            }

            return null;
        }

        /// <summary>
        /// Get value by the key.
        /// </summary>
        /// <param name="xmlNode">The XmlNode instance.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value</returns>
        /// <remarks>
        /// This method is searching value in both attributes and children nodes.
        /// </remarks>
        public static string GetNodeValue(this XmlNode xmlNode, string key)
        {
            string result = xmlNode.GetAttributeValue(key);

            if (string.IsNullOrEmpty(result))
                result = xmlNode.GetElementValue(key);

            return result;
        }
    }
}
