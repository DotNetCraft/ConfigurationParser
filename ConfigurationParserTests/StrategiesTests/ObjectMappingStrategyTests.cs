using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ConfigurationParser;
using ConfigurationParser.Mapping;
using ConfigurationParser.Mapping.Strategies;
using ConfigurationParser.Mapping.Strategies.Implementation;
using NSubstitute;
using NUnit.Framework;

namespace ConfigurationParserTests.StrategiesTests
{
    [TestFixture]
    class ObjectMappingStrategyTests
    {
        public class TestClass
        {
            public string A { get; set; }
        }

        [Test]
        [TestCase(@"<TestClass A=""test""/>", typeof(TestClass))]
        public void ObjectMappingTest(string input, Type expectedType)
        {
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IConfigurationReader configurationParser = Substitute.For<IConfigurationReader>();
            configurationParser.ReadObject(expectedType, xmlNode).Returns(new TestClass {A = "test"});

            IMappingStrategy mappingStrategy = new ObjectMappingStrategy();            
            var actual = mappingStrategy.Map(xmlNode, expectedType, configurationParser);
            Assert.AreEqual(expectedType, actual.GetType());
        }        
    }
}
