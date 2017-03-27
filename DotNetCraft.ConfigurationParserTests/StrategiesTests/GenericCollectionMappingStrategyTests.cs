using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DotNetCraft.ConfigurationParser;
using DotNetCraft.ConfigurationParser.Mapping;
using DotNetCraft.ConfigurationParser.Mapping.Strategies;
using DotNetCraft.ConfigurationParser.Mapping.Strategies.Implementation;
using NSubstitute;
using NUnit.Framework;

namespace DotNetCraft.ConfigurationParserTests.StrategiesTests
{
    [TestFixture]
    class GenericCollectionMappingStrategyTests
    {
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

        [Test]
        [TestCase(@"<List>
                        <item>a</item>
                        <item>b</item>
                        <item>c</item>
                    </List>", typeof(IList<string>), typeof(string), 3)]
        [TestCase(@"<List>
                        <item>1</item>
                        <item>2</item>
                        <item>3</item>
                    </List>", typeof(IList<int>), typeof(int), 3)]
        [TestCase(@"<List>
                        <item>1</item>
                        <item>2</item>
                        <item>3</item>
                    </List>", typeof(IList<double>), typeof(double), 3)]
        [TestCase(@"<List>
                        <item>a</item>
                        <item>b</item>
                        <item>c</item>
                    </List>", typeof(ICollection<string>), typeof(string), 3)]
        [TestCase(@"<List>
                        <item>1</item>
                        <item>2</item>
                        <item>3</item>
                    </List>", typeof(ICollection<int>), typeof(int), 3)]
        [TestCase(@"<List>
                        <item>1</item>
                        <item>2</item>
                        <item>3</item>
                    </List>", typeof(ICollection<double>), typeof(double), 3)]
        public void InterfaceGenericCollectionMappingWithPrimitiveItemsTest(string input, Type expectedType, Type itemType, int collectionSize)
        {
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IConfigurationReader configurationParser = Substitute.For<IConfigurationReader>();
            IMappingStrategyFactory mappingStrategyFactory = Substitute.For<IMappingStrategyFactory>();

            mappingStrategyFactory.CreatePrimitiveStrategy(itemType).Returns(new PrimitiveMappingStrategy());

            IMappingStrategy mappingStrategy = new GenericCollectionMappingStrategy(mappingStrategyFactory);
            var actual = mappingStrategy.Map(xmlNode.FirstChild, expectedType, configurationParser);
            Assert.IsNotNull(actual.GetType().GetInterfaces().SingleOrDefault(x=>x == expectedType));
            Assert.AreEqual(collectionSize, ((ICollection)actual).Count);
        }
    }
}
