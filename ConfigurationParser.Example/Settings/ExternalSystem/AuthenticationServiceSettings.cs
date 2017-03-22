using System.Collections.Generic;

namespace ConfigurationParser.Example.Settings.ExternalSystem
{
    class AuthenticationServiceSettings
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<string> Urls { get; set; }
    }
}
