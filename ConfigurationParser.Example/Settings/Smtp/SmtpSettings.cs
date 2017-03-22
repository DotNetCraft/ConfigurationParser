using System.Collections.Generic;
using ConfigurationParser.Attributes;
using ConfigurationParser.Example.CustomStrategies;

namespace ConfigurationParser.Example.Settings.Smtp
{
    class SmtpSettings
    {
        public string Host { get; set; }
        public string Sender { get; set; }

        [CustomStrategy(typeof(SplitRecipientsCustomStrategy))]
        public List<string> Recipients { get; set; }
    }
}
