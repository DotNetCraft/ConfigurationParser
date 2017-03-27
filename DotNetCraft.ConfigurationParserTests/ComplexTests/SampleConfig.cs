using System.Collections.Generic;
using DotNetCraft.ConfigurationParser.Attributes;

namespace DotNetCraft.ConfigurationParserTests.ComplexTests
{
    public class SampleConfig
    {
        public string Name { get; set; }
        public int MaxValue { get; set; }

        [PropertyMapping("Different")]
        public string AttrName { get; set; }

        public int AttrMaxValue { get; set; }

        public SomeEnum Some { get; set; }

        public InnerSettings Inner { get; set; }

        public List<string> StringList { get; set; }
        public Dictionary<int, string> DictionaryList { get; set; }
        public List<InnerSettings> InnerList { get; set; }

        public Dictionary<string, string> DictionaryList2 { get; set; }

        public Dictionary<string, InnerSettings> SuperDictionaryList { get; set; }

        public string[] JustStringArray { get; set; }

        [CustomStrategy(typeof(SimpleCustomStrategy))]
        public int[] CustomStrategy { get; set; }

        [PropertyMapping("DifferentCustomStrategy")]
        [CustomStrategy(typeof(SimpleCustomStrategy))]
        public int[] CustomStrategy2 { get; set; }

        public string MissingConfig { get; set; }
    }

    public class InnerSettings
    {
        public SomeEnum InnerSome { get; set; }
        public string InnerName { get; set; }
    }

    public enum SomeEnum
    {
        One, Two, Three
    }

    public class SameSampleConfig : SampleConfig
    {

    }
}
