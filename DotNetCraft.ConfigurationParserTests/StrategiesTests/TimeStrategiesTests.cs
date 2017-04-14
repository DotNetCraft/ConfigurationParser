using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class TimeStrategiesTests
    {
        [Test]
        [TestCase(@"<item>00:30:00</item>", typeof(TimeSpan), typeof(TimeSpan), "00:30:00")]
        public void TimeSpanMappingTest(string input, Type expectedType, Type itemType, string expected)
        {
            TimeSpan expectedTimeSpan = TimeSpan.Parse(expected);
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IConfigurationReader configurationParser = Substitute.For<IConfigurationReader>();
            IMappingStrategy mappingStrategy = new TimeSpanStrategy();
            var actual = mappingStrategy.Map(xmlNode.FirstChild, expectedType, configurationParser);
            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreEqual(expectedTimeSpan, ((TimeSpan)actual));
        }

        [Test]
        [TestCase(@"<item>01.02.03</item>", typeof(DateTime), typeof(DateTime), "01.02.03")]
        public void DateTimeMappingTest(string input, Type expectedType, Type itemType, string expected)
        {
            DateTime expectedDate = DateTime.Parse(expected);
            XmlDocument xmlNode = new XmlDocument();
            xmlNode.LoadXml(input);

            IConfigurationReader configurationParser = Substitute.For<IConfigurationReader>();
            IMappingStrategy mappingStrategy = new DateTimeStrategy();
            var actual = mappingStrategy.Map(xmlNode.FirstChild, expectedType, configurationParser);
            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreEqual(expectedDate, ((DateTime)actual));
        }
    }
}
