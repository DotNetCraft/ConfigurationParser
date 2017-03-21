using System;
using System.Configuration;
using System.Xml;
using ConfigurationParser;
using ConfigurationParser.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ConfigurationParserTests
{
    [TestClass]
    [TestFixture]
    public class ConfigurationReaderTests
    {
        private class SimpleConfiguration
        {
            public string Name { get; set; }
            public int MaxValue { get; set; }
        }

        [TestMethod]
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

        [TestMethod]
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
