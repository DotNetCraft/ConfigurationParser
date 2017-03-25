using System.Configuration;
using System.Xml;
using ConfigurationParser.Mapping;

namespace ConfigurationParser
{
    /// <summary>
    /// Configuration section handler.
    /// </summary>
    public class SimpleConfigurationSectionHandler: IConfigurationSectionHandler
    {
        #region Implementation of IConfigurationSectionHandler

        /// <summary>Creates a configuration section handler.</summary>
        /// <returns>The created section handler object.</returns>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        public object Create(object parent, object configContext, XmlNode section)
        {
            IMappingStrategyFactory mappingStrategyFactory = new MappingStrategyFactory();
            IConfigurationReader configurationReader = new ConfigurationReader(section, mappingStrategyFactory);
            return configurationReader;
        }

        #endregion
    }
}
