using System;
using System.ComponentModel;
using ConfigurationParser.Mapping;
using ConfigurationParser.Mapping.Strategies;
using ConfigurationParser.Mapping.Strategies.Implementation;
using NUnit.Framework;

namespace ConfigurationParserTests.StrategiesTests
{
    [TestFixture]
    public class PrimitiveMappingStrategyTests
    {
        public enum TestEnum
        {
            One,
            Two,
            Three
        }

        public class TestClass
        {
            public string A { get; set; }
        }

        [Test]
        [TestCase("1", typeof(int))]
        [TestCase("1", typeof(Int64))]
        [TestCase("1", typeof(short))]
        [TestCase("1", typeof(float))]
        [TestCase("1", typeof(double))]
        [TestCase("string", typeof(string))]
        [TestCase("{8E0084F1-C9B4-4E2F-AE6E-C04DD110DE3C}", typeof(Guid))]
        [TestCase("Two", typeof(TestEnum))]
        public void PrimitiveTypesMappingTest(string input, Type expectedType)
        {
            IPrimitiveMappingStrategy primitiveMappingStrategy = new PrimitiveMappingStrategy();
            var actual = primitiveMappingStrategy.Map(input, expectedType);
            Assert.AreEqual(expectedType, actual.GetType());
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void WrongTypeMappingTest()
        {
            IPrimitiveMappingStrategy primitiveMappingStrategy = new PrimitiveMappingStrategy();
            primitiveMappingStrategy.Map("some data", typeof(TestClass));            
        }
    }
}
