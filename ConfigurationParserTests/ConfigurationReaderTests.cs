﻿using System.Xml;
using ConfigurationParser;
using ConfigurationParser.Mapping;
using NUnit.Framework;

namespace ConfigurationParserTests
{
    [TestFixture]
    public class ConfigurationReaderTests
    {
        private class SimpleConfiguration
        {
            public string Name { get; set; }
            public int MaxValue { get; set; }
        }

        [Test]
        public void ReadConfigurationAttributesTest()
        {
            string inputXml = @"
                <SimpleConfiguration Name=""AAA"" MaxValue=""23"">
                </SimpleConfiguration>";

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(inputXml);

            IMappingStrategyFactory mappingStrategyFactory = MappingStrategyFactory.Instance;
            IConfigurationReader configurationReader = new ConfigurationReader(xml.FirstChild, mappingStrategyFactory);

            SimpleConfiguration actual = configurationReader.ReadObject<SimpleConfiguration>();
            Assert.AreEqual(23, actual.MaxValue);
            Assert.AreEqual("AAA", actual.Name);
        }

        [Test]
        public void ReadConfiguratioNodesTest()
        {
            string inputXml = @"
                <SimpleConfiguration>
                    <Name>AAA</Name>
                    <MaxValue>23</MaxValue>
                </SimpleConfiguration>";

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(inputXml);

            IMappingStrategyFactory mappingStrategyFactory = MappingStrategyFactory.Instance;
            IConfigurationReader configurationReader = new ConfigurationReader(xml.FirstChild, mappingStrategyFactory);

            SimpleConfiguration actual = configurationReader.ReadObject<SimpleConfiguration>();
            Assert.AreEqual(23, actual.MaxValue);
            Assert.AreEqual("AAA", actual.Name);
        }
    }
}
