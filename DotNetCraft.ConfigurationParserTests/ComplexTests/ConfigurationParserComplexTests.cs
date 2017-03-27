using System.IO;
using System.Xml;
using DotNetCraft.ConfigurationParser;
using DotNetCraft.ConfigurationParser.Mapping;
using NUnit.Framework;

namespace DotNetCraft.ConfigurationParserTests.ComplexTests
{
    [TestFixture]
    class ConfigurationParserComplexTests
    {
        [Test]
        [TestCase("ComplexTests\\ComplexTestData01.xml")]
        [TestCase("ComplexTests\\ComplexTestData02.xml")]
        public void ReadConfigurationComplexTest(string fileName)
        {
            string input = File.ReadAllText(fileName);
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IMappingStrategyFactory mappingStrategyFactory = new MappingStrategyFactory();
            IConfigurationReader configurationReader = new ConfigurationReader(xmlNode.FirstChild, mappingStrategyFactory);

            SampleConfig actual = configurationReader.ReadObject<SampleConfig>();
            Assert.AreEqual(3, actual.SuperDictionaryList.Count);
            Assert.AreEqual(3, actual.DictionaryList2.Count);
        }
    }
}
