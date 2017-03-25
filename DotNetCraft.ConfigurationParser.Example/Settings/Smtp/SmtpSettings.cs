using System.Collections.Generic;
using ConfigurationParser.Attributes;
using DotNetCraft.ConfigurationParser.Example.CustomStrategies;

namespace DotNetCraft.ConfigurationParser.Example.Settings.Smtp
{
    class SmtpSettings
    {
        public string Host { get; set; }
        public string Sender { get; set; }

        [CustomStrategy(typeof(SplitRecipientsCustomStrategy))]
        public List<string> Recipients { get; set; }
    }
}
