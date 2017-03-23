using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using ConfigurationParser;
using ConfigurationParser.Mapping;
using ConfigurationParser.Mapping.Strategies.Implementation;
using NSubstitute;
using NUnit.Framework;

namespace ConfigurationParserTests.StrategiesTests
{
    [TestFixture]
    class GenericCollectionMappingStrategyTests
    {
        public class TestClass
        {
            public string A { get; set; }
        }

        [Test]
        [TestCase(@"<List>
                        <item>a</item>
                        <item>b</item>
                        <item>c</item>
                    </List>", typeof(List<string>), typeof(string), 3)]
        [TestCase(@"<List>
                        <item>1</item>
                        <item>2</item>
                        <item>3</item>
                    </List>", typeof(List<int>), typeof(int), 3)]
        [TestCase(@"<List>
                        <item>1</item>
                        <item>2</item>
                        <item>3</item>
                    </List>", typeof(List<double>), typeof(double), 3)]
        public void GenericCollectionMappingWithPrimitiveItemsTest(string input, Type expectedType, Type itemType, int collectionSize)
        {
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IConfigurationReader configurationParser = Substitute.For<IConfigurationReader>();
            IMappingStrategyFactory mappingStrategyFactory = Substitute.For<IMappingStrategyFactory>();

            mappingStrategyFactory.CreatePrimitiveStrategy(itemType).Returns(new PrimitiveMappingStrategy());

            IMappingStrategy mappingStrategy = new GenericCollectionMappingStrategy(mappingStrategyFactory);
            var actual = mappingStrategy.Map(xmlNode.FirstChild, expectedType, configurationParser);
            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreEqual(collectionSize, ((ICollection)actual).Count);
        }
    }
}
