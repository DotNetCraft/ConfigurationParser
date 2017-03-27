using System.IO;
using System.Reflection;
using System.Xml;
using DotNetCraft.ConfigurationParser;
using DotNetCraft.ConfigurationParser.Mapping;
using NUnit.Framework;

namespace DotNetCraft.ConfigurationParserTests.ComplexTests
{
    [TestFixture]
    class ConfigurationParserComplexTests
    {
        private static string ReadFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("DotNetCraft.ConfigurationParserTests.ComplexTests." + fileName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        [Test]
        [TestCase("ComplexTestData01.xml")]
        [TestCase("ComplexTestData02.xml")]
        public void ReadConfigurationComplexTest(string fileName)
        {
            string input = ReadFile(fileName);
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
