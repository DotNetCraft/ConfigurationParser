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
    class DictionaryMappingStrategyTests
    {      
        [Test]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""a""/>
                        <item key=""2"" value=""c""/>
                        <item key=""3"" value=""b""/>
                    </Dictionary>", typeof(Dictionary<int, string>), typeof(string), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(Dictionary<int, int>), typeof(int), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(Dictionary<string, double>), typeof(double), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""a""/>
                        <item key=""2"" value=""c""/>
                        <item key=""3"" value=""b""/>
                    </Dictionary>", typeof(SortedDictionary<int, string>), typeof(string), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(SortedDictionary<int, int>), typeof(int), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(SortedDictionary<string, double>), typeof(double), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""a""/>
                        <item key=""2"" value=""c""/>
                        <item key=""3"" value=""b""/>
                    </Dictionary>", typeof(SortedList<int, string>), typeof(string), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(SortedList<int, int>), typeof(int), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(SortedList<string, double>), typeof(double), 3)]
        public void DictionaryMappingStrategyWithPrimitiveItemsTest(string input, Type expectedType, Type itemType, int collectionSize)
        {
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IConfigurationReader configurationParser = Substitute.For<IConfigurationReader>();
            IMappingStrategyFactory mappingStrategyFactory = Substitute.For<IMappingStrategyFactory>();

            mappingStrategyFactory.CreatePrimitiveStrategy(itemType).Returns(new PrimitiveMappingStrategy());

            IMappingStrategy mappingStrategy = new GenericDictionaryMappingStrategy(mappingStrategyFactory);
            var actual = mappingStrategy.Map(xmlNode.FirstChild, expectedType, configurationParser);
            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreEqual(collectionSize, ((IDictionary)actual).Count);
        }

        [Test]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""a""/>
                        <item key=""2"" value=""c""/>
                        <item key=""3"" value=""b""/>
                    </Dictionary>", typeof(IDictionary<int, string>), typeof(string), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(IDictionary<int, int>), typeof(int), 3)]
        [TestCase(@"<Dictionary>
                        <item key=""1"" value=""1""/>
                        <item key=""2"" value=""2""/>
                        <item key=""3"" value=""3""/>
                    </Dictionary>", typeof(IDictionary<string, double>), typeof(double), 3)]
        public void InterfaceDictionaryMappingStrategyWithPrimitiveItemsTest(string input, Type expectedType, Type itemType, int collectionSize)
        {
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IConfigurationReader configurationParser = Substitute.For<IConfigurationReader>();
            IMappingStrategyFactory mappingStrategyFactory = Substitute.For<IMappingStrategyFactory>();

            mappingStrategyFactory.CreatePrimitiveStrategy(itemType).Returns(new PrimitiveMappingStrategy());

            IMappingStrategy mappingStrategy = new GenericDictionaryMappingStrategy(mappingStrategyFactory);
            var actual = mappingStrategy.Map(xmlNode.FirstChild, expectedType, configurationParser);
            Assert.IsNotNull(actual.GetType().GetInterfaces().SingleOrDefault(x => x == expectedType));
            Assert.AreEqual(collectionSize, ((IDictionary)actual).Count);
        }
    }
}
